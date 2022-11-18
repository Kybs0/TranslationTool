using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace TranslationTool.Helper
{
    public static class AnimationBuilder
    {
        public static Timeline GetTranslateXAnimation(UIElement target, TimeSpan timeSpan, double to, double? fromValue = null)
        {
            var animation = new DoubleAnimationUsingKeyFrames();
            if (fromValue != null)
            {
                var beginFrame = new EasingDoubleKeyFrame((double)fromValue, KeyTime.FromTimeSpan(TimeSpan.Zero));
                animation.KeyFrames.Add(beginFrame);
            }
            var edkf = new EasingDoubleKeyFrame(to, KeyTime.FromTimeSpan(timeSpan), new PowerEase { EasingMode = EasingMode.EaseOut });
            animation.KeyFrames.Add(edkf);

            var hasGroup = target.RenderTransform is TransformGroup;
            if (!hasGroup)
            {
                var transformGroup = new TransformGroup();
                transformGroup.Children.Add(new ScaleTransform());
                transformGroup.Children.Add(new SkewTransform());
                transformGroup.Children.Add(new RotateTransform());
                transformGroup.Children.Add(new TranslateTransform());
                target.RenderTransform = transformGroup;
            }

            // 给dakf设置目标和属性
            Storyboard.SetTarget(animation, target);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[3].(TranslateTransform.X)"));

            return animation;
        }
    }
    public static class MotionExtensions
    {

        public static void StartTranslateX(this UIElement target, TimeSpan timeSpan, double to, double? from = null)
        {
            var animation = AnimationBuilder.GetTranslateXAnimation(target, timeSpan, to, from);
            var storyboard = new Storyboard();
            storyboard.Children.Add(animation);

            storyboard.Begin();
        }
    }
}
