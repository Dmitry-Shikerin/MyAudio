using System;
using Doozy.Editor.EditorUI;
using Doozy.Editor.EditorUI.Utils;
using Doozy.Editor.Reactor.Internal;
using Doozy.Runtime.Colors;
using Doozy.Runtime.Common.Events;
using Doozy.Runtime.Reactor.Easings;
using Doozy.Runtime.Reactor.Internal;
using Doozy.Runtime.Reactor.Reactions;
using Doozy.Runtime.UIElements.Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace MyAudios.Soundy.Editor.DataBases.Editors.UXMLs
{
    public class FluidMinMaxSlider : VisualElement
    {
        public VisualElement labelsContainer { get; }
        public VisualElement snapIntervalIndicatorsContainer { get; }
        public VisualElement snapValuesIndicatorsContainer { get; }
        public VisualElement sliderContainer { get; }
        public MinMaxSlider slider { get; }

        public Label lowValueLabel { get; private set; }
        public Label highValueLabel { get; private set; }
        public Label valueLabel { get; private set; }

        public bool snapToInterval { get; set; } = true;
        public float snapInterval { get; set; } = 0.1f;

        public bool snapToValues { get; set; } = false;
        public float[] snapValues { get; set; } = { 0.1f, 0.5f, 1f, 2f, 5f, 10f };
        public float snapValuesInterval { get; set; } = 0.1f;

        public bool autoResetToValue { get; set; } = false;
        public float autoResetValue { get; set; } = 0f;

        public UnityEvent onStartValueChange { get; } = new UnityEvent();
        public UnityEvent onEndValueChange { get; } = new UnityEvent();
        public FloatEvent onMinValueChanged { get; } = new FloatEvent();
        public FloatEvent onMaxValueChanged { get; } = new FloatEvent();
        public Action<Vector2> onValueChanged;

        private FloatReaction resetToValueReaction { get; set; }

        public VisualElement sliderTracker { get; }
        public VisualElement sliderDraggerBorder { get; }
        public VisualElement sliderDragger { get; }

        private const float TRACKER_OFFSET = 4;
        private float sliderTrackerWidth => sliderTracker.resolvedStyle.width - TRACKER_OFFSET * 2;
        // private float length => slider.highValue - slider.lowValue;

        public FluidMinMaxSlider()
        {
            this
                .SetStyleFlexShrink(0)
                .SetStyleFlexGrow(1)
                .SetStylePaddingLeft(DesignUtils.k_Spacing2X)
                .SetStylePaddingRight(DesignUtils.k_Spacing2X)
                .RegisterCallback<GeometryChangedEvent>(evt =>
                {
                    // UpdateSnapValuesIndicators();
                });

            labelsContainer =
                new VisualElement()
                    .SetName("Labels Container")
                    .SetStyleFlexDirection(FlexDirection.Row)
                    .SetStyleJustifyContent(Justify.Center)
                    .SetStyleAlignItems(Align.FlexStart)
                    .SetStyleMarginTop(4)
                    .SetStyleFlexGrow(1);

            sliderContainer =
                new VisualElement()
                    .SetStyleHeight(28)
                    .SetName("Slider Container")
                    .SetStyleFlexGrow(1)
                    .SetStylePaddingTop(8)
                ;

            snapIntervalIndicatorsContainer =
                new VisualElement()
                    .SetName("Snap Interval Indicators Container")
                    .SetStylePosition(Position.Absolute)
                    .SetStyleLeft(0)
                    .SetStyleTop(0)
                    .SetStyleRight(0)
                    .SetStyleBottom(0);

            snapValuesIndicatorsContainer =
                new VisualElement()
                    .SetName("Snap Values Indicators Container")
                    .SetStylePosition(Position.Absolute)
                    .SetStyleLeft(0)
                    .SetStyleTop(0)
                    .SetStyleRight(0)
                    .SetStyleBottom(0);

            slider =
                new MinMaxSlider()
                    .ResetLayout();

            sliderTracker = slider.Q<VisualElement>("unity-tracker");
            sliderDraggerBorder = slider.Q<VisualElement>("unity-dragger-border");
            sliderDragger = slider.Q<VisualElement>("unity-dragger");
            
            sliderDragger.SetStyleBorderColor(EditorColors.Default.BoxBackground.WithRGBShade(0.4f));

            // FloatReaction sliderDraggerBorderReaction =
            //     Reaction.Get<FloatReaction>()
            //         .SetEditorHeartbeat()
            //         .SetGetter(() => sliderDraggerBorder.GetStyleWidth())
            //         .SetSetter(value =>
            //         {
            //             sliderDraggerBorder.SetStyleSize(value);
            //             float positionOffset = 5 - value * 0.25f;
            //             sliderDraggerBorder.SetStyleLeft(positionOffset);
            //             sliderDraggerBorder.SetStyleTop(positionOffset);
            //         })
            //         .SetDuration(0.15f)
            //         .SetEase(Ease.OutSine);

            // sliderDraggerBorderReaction.SetFrom(0f);
            // sliderDraggerBorderReaction.SetTo(16f);
            //
            // sliderDragger.RegisterCallback<PointerEnterEvent>(evt => sliderDraggerBorderReaction?.Play());
            // sliderDragger.RegisterCallback<PointerLeaveEvent>(evt => sliderDraggerBorderReaction?.Play(PlayDirection.Reverse));

            resetToValueReaction =
                Reaction
                    .Get<FloatReaction>()
                    .SetEditorHeartbeat()
                    .SetDuration(0.34f)
                    .SetEase(Ease.OutExpo)
                // .SetGetter(() => slider.value)
                // .SetSetter(value =>
                // {
                //     slider.SetValueWithoutNotify(value);
                //     if (valueLabel != null)
                //     {
                //         valueLabel.text = value.RoundToMultiple(snapInterval).ToString(CultureInfo.InvariantCulture);
                //     }
                // })
                ;

            // slider.RegisterValueChangedCallback(evt =>
            // {
            //     if (evt?.newValue == null)
            //         return;
            //     
            //     float value = evt.newValue;
            //
            //     bool snappedToValue = false;
            //     
            //     if (snapToValues)
            //     {
            //         foreach (float snapValue in snapValues)
            //         {
            //             if (Math.Abs(snapValue - value) <= snapValuesInterval)
            //             {
            //                 value = snapValue;
            //                 snappedToValue = true;
            //                 
            //                 break;
            //             }
            //         }
            //     }
            //
            //     if (!snappedToValue && snapToInterval)
            //     {
            //         value = evt.newValue.RoundToMultiple(snapInterval);
            //     }
            //
            //     if (valueLabel != null)
            //     {
            //         valueLabel.text = value.ToString(CultureInfo.InvariantCulture);
            //     }
            //
            //     slider.SetValueWithoutNotify(value);
            //     onValueChanged?.Invoke(value);
            // });
            //
            // slider.RegisterCallback<PointerCaptureEvent>(evt =>
            // {
            //     if (autoResetToValue & !slider.value.CloseTo(autoResetValue, 0.01f))
            //         resetToValueReaction?.SetProgressAtOne();
            //
            //     onStartValueChange?.Invoke();
            //     
            //     sliderDraggerBorderReaction?.Play(PlayDirection.Forward);
            // });
            //
            // slider.RegisterCallback<PointerCaptureOutEvent>(evt =>
            // {
            //     onEndValueChange?.Invoke();
            //     sliderDraggerBorderReaction?.Play(PlayDirection.Reverse);
            //
            //     if (!autoResetToValue) return;
            //     resetToValueReaction.SetFrom(slider.value);
            //     resetToValueReaction.SetTo(autoResetValue);
            //     resetToValueReaction.Play();
            //
            // });
            
            slider.RegisterValueChangedCallback((evt) =>
            {
                onValueChanged?.Invoke(evt.newValue);
            });

            Initialize();
            Compose();
        }

        // public FluidMinMaxSlider(float lowValue, float highValue) : this() =>
        //     this.SetSliderLowAndHighValues(lowValue, highValue);

        private void Initialize()
        {
            Label GetLabel() =>
                DesignUtils.fieldLabel
                    .SetStyleMinWidth(40)
                    .SetStyleWidth(40)
                    .SetStyleFontSize(11);

            valueLabel =
                GetLabel()
                    .SetStyleColor(EditorColors.Default.UnityThemeInversed)
                    .SetStyleTextAlign(TextAnchor.LowerCenter)
                    .SetStyleFontSize(13);

            lowValueLabel = GetLabel().SetStyleTextAlign(TextAnchor.UpperLeft);
            highValueLabel = GetLabel().SetStyleTextAlign(TextAnchor.UpperRight);

            labelsContainer
                .AddChild(lowValueLabel)
                .AddChild(DesignUtils.flexibleSpace)
                .AddChild(valueLabel)
                .AddChild(DesignUtils.flexibleSpace)
                .AddChild(highValueLabel);

            sliderContainer
                .SetStyleHeight(20)
                .AddChild(snapIntervalIndicatorsContainer)
                .AddChild(snapValuesIndicatorsContainer)
                .AddChild(slider);
        }

        private void Compose()
        {
            this
                .AddChild(sliderContainer)
                .AddChild(labelsContainer);
        }

        // internal List<float> CleanValues(IEnumerable<float> values)
        // {
        //     float minValue = Mathf.Min(slider.lowValue, slider.highValue);
        //     float maxValue = Mathf.Max(slider.lowValue, slider.highValue);
        //     var list = values.Where(v => v >= minValue && v <= maxValue).ToList();
        //     list.Sort();
        //     return list;
        // }

        // internal void UpdateSnapValuesIndicators()
        // {
        //     snapValuesIndicatorsContainer.Clear();
        //     
        //     if (!snapToValues)
        //         return;
        //     
        //     if (float.IsNaN(sliderTrackerWidth))
        //         return;
        //     
        //     foreach (float snapValue in CleanValues(snapValues))
        //     {
        //         float normalizedValue = (snapValue - slider.lowValue) / length;
        //         float position = normalizedValue * sliderTrackerWidth + TRACKER_OFFSET;
        //
        //         Label label =
        //             DesignUtils.fieldLabel
        //                 .SetText(snapValue.ToString(CultureInfo.InvariantCulture))
        //                 .SetStyleMarginBottom(2)
        //                 .SetStyleTextAlign(TextAnchor.MiddleCenter)
        //                 .SetStyleWidth(30)
        //                 .SetStyleMarginLeft(-14);
        //
        //         snapValuesIndicatorsContainer
        //             .AddChild
        //             (
        //                 new VisualElement()
        //                     .SetName($"Snap Value Indicator: {snapValue}")
        //                     .SetStylePosition(Position.Absolute)
        //                     .SetStyleLeft(position)
        //                     .AddChild(label)
        //                     .AddChild
        //                     (
        //                         DesignUtils.dividerVertical
        //                             .SetStyleMargins(0)
        //                             .SetStyleHeight(8)
        //                     )
        //             );
        //
        //         if (snapValue.Approximately(slider.lowValue) || snapValue.Approximately(slider.highValue))
        //         {
        //             label.visible = false;
        //         }
        //     }
        //}
    }
}