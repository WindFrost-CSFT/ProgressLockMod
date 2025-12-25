using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.ModLoader.Config.UI;
using Terraria.UI;

public class ScrollableEnumElement : ConfigElement
{
    private UIPanel headerPanel;
    private UIText headerText;
    private bool popupOpen = false;
    // internal static UIText arrow;
    //internal static bool popupOpen;
    public static EnumPopupWindow popupWindow;

    public override void SetObject(object value)
    {
        base.SetObject(value);
        UpdateHeaderText();      // 只做 UI，同步显示
    }
    public override void OnBind()
    {
        base.OnBind();

        Height.Set(30f, 0f);

        headerPanel = new UIPanel();
        headerPanel.HAlign = 1f;
        headerPanel.VAlign = 0.5f;
        headerPanel.Width.Set(-288f, 1f);
        headerPanel.Height.Set(30f, 0f);
        headerPanel.BackgroundColor = new Color(63, 82, 151) * 0f;
        headerPanel.OnLeftClick += TogglePopup;
        Append(headerPanel);

        headerText = new UIText("点击选择...");
        headerText.HAlign = 1f;
        headerText.VAlign = 0.5f;
        headerText.Left.Set(0f, 0f);
        headerText.Top.Set(0f, 0f);
        headerPanel.Append(headerText);


        var arrow = new UIText("☰▾");  // Unicode 下拉箭头
        arrow.HAlign = 1f;            // 靠右对齐
        arrow.Left.Set(-140f, 0f);
        arrow.VAlign = 0.5f;          // 垂直居中
        arrow.TextColor = Color.White;
        headerPanel.Append(arrow);
        UpdateHeaderText();

    }
    private void TogglePopup(UIMouseEvent evt, UIElement listeningElement)
    {
        if (popupOpen)
        {
            ClosePopup();
            return;
        }

        OpenPopup(evt, listeningElement);
    }
    private void OpenPopup(UIMouseEvent evt, UIElement listeningElement)
    {

        popupOpen = true;

        if (popupWindow != null)
        {
            ClosePopup();
        }

        var enumType = MemberInfo.Type;
        var enumValues = Enum.GetValues(enumType);

        popupWindow = new EnumPopupWindow(this, enumValues);

        CalculatedStyle dimensions = GetDimensions();
        float x = dimensions.X + 260f;
        float y = dimensions.Y + 35f;

        popupWindow.Left.Set(x, 0f);
        popupWindow.Top.Set(y, 0f);
        popupWindow.Recalculate();

        UIElement root = this;
        while (root.Parent != null) root = root.Parent;
        root.Append(popupWindow);
    }

    public static void ClosePopup()
    {
        if (popupWindow != null)
        {
            popupWindow.parent.popupOpen = false;
            popupWindow.Remove();
            popupWindow = null;
        }

        if (popupWindow != null && popupWindow.Parent != null)
        {
            popupWindow.Parent.RemoveChild(popupWindow);
            popupWindow = null;
        }
    }

    /// <summary>
    /// 设置值并通知 tModLoader 配置已修改
    /// </summary>
    public void SetValue(object value)
    {
        if (value != null)
        {
            SetObject(value);
            ClosePopup();
        }

    }

    public object GetValue()
    {
        return MemberInfo?.GetValue(Item);
    }

    // 记得添加这个引用

    private void UpdateHeaderText()
    {
        if (headerText == null || headerPanel == null) return;

        var currentValue = GetValue();
        string text = "点击选择...";

        if (currentValue != null)
        {
            // 核心改动：获取本地化键并获取翻译
            // TML 枚举本地化路径格式通常为: Mods.ModName.Configs.EnumTypeName.EnumMemberName.Label
            string localizationKey = $"Mods.ProgressLock.Configs.{currentValue.GetType().Name}.{currentValue}.Label";
            text = Terraria.Localization.Language.GetTextValue(localizationKey);

            // 如果找不到翻译（返回了键本身），则回退到 ToString()
            if (text == localizationKey)
                text = currentValue.ToString();
        }

        // ... 后续计算缩放的代码保持不变 ...
        float currentWidth = headerPanel.GetInnerDimensions().Width;
        if (currentWidth <= 0) currentWidth = 150f;
        float availableWidth = Math.Max(10f, currentWidth - 35f);
        Vector2 stringSize = FontAssets.MouseText.Value.MeasureString(text);
        float scale = stringSize.X > availableWidth ? Math.Max(0.1f, availableWidth / stringSize.X) : 1f;

        headerText.SetText(text, scale, false);
    }
}

// 弹窗类
public class EnumPopupWindow : UIPanel
{
    private UIList list;
    private UIScrollbar scrollbar;
    internal ScrollableEnumElement parent;


    public EnumPopupWindow(ScrollableEnumElement parent, Array enumValues)
    {
        this.parent = parent;

        Width.Set(300f, 0f);
        Height.Set(300f, 0f);

        BackgroundColor = new Color(33, 43, 79) * 0.95f;
        BorderColor = Color.Black;

        var closeButton = new UIText("✕");
        closeButton.TextColor = Color.Red;
        closeButton.HAlign = 1f;
        closeButton.Top.Set(5f, 0f);
        closeButton.Left.Set(-10f, 0f);
        closeButton.OnLeftClick += (evt, elm) => ScrollableEnumElement.ClosePopup();
        Append(closeButton);

        var title = new UIText("选择一项:");
        title.Top.Set(5f, 0f);
        title.Left.Set(10f, 0f);
        Append(title);

        list = new UIList();
        list.Width.Set(-25f, 1f);
        list.Height.Set(-35f, 1f);
        list.Top.Set(30f, 0f);
        list.Left.Set(5f, 0f);
        list.ListPadding = 2f;
        Append(list);

        scrollbar = new UIScrollbar();
        scrollbar.SetView(100f, 1000f);
        scrollbar.Height.Set(-35f, 1f);
        scrollbar.Top.Set(30f, 0f);
        scrollbar.HAlign = 1f;
        Append(scrollbar);

        list.SetScrollbar(scrollbar);

        foreach (var value in enumValues)
        {
            var element = new EnumOptionElement(parent, value);
            list.Add(element);
        }
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);



        if (Main.mouseLeft && Main.mouseLeftRelease)
        {
            CalculatedStyle dimensions = GetDimensions();
            if (!dimensions.ToRectangle().Contains(Main.MouseScreen.ToPoint()))
            {
                ScrollableEnumElement.ClosePopup();
            }
        }
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);

        CalculatedStyle dimensions = GetOuterDimensions();
        Rectangle rect = dimensions.ToRectangle();
        rect.Inflate(2, 2);

        Texture2D pixel = new Texture2D(Main.graphics.GraphicsDevice, 1, 1);
        pixel.SetData(new[] { Color.White });

        spriteBatch.Draw(pixel, rect, Color.Black * 0f);
    }
}

// 枚举选项
public class EnumOptionElement : UIPanel
{
    private readonly ScrollableEnumElement parent;
    private readonly object value;
    private readonly string displayName;
    private UIText textElement;

    public EnumOptionElement(ScrollableEnumElement parent, object value)
    {
        this.parent = parent;
        this.value = value;

        // --- 本地化适配 ---
        string modName = "ProgressLock";
        string localizationKey = $"Mods.{modName}.Configs.{value.GetType().Name}.{value}.Label";
        string translatedName = Terraria.Localization.Language.GetTextValue(localizationKey);
        this.displayName = (translatedName == localizationKey) ? value.ToString() : translatedName;

        // --- 面板基础设置 ---
        Width.Set(0f, 1f);
        Height.Set(28f, 0f);

        // 【核心修正 1】 必须调用这个！彻底杀掉 UIPanel 默认的 12px 内边距
        // 不杀掉这个，VAlign 计算出来的中心点永远是偏的
        SetPadding(0f);

        BackgroundColor = new Color(63, 82, 151) * 0.7f;
        BorderColor = new Color(89, 116, 213) * 0.5f;

        // --- 文字元素设置 ---
        // 【核心修正 2】 使用非精简版的 UIText 构造函数，或者确保没被缩放干扰
        textElement = new UIText(displayName);

        // 强制左对齐
        textElement.HAlign = 0f;
        // 强制垂直居中
        textElement.VAlign = 0.5f;

        // 【核心修正 3】 放弃 Padding，全用 Left/Top 坐标控制
        // 如果还是觉得靠上，把下面的 2f 改成 4f 或 5f，直到它下来为止
        textElement.Left.Set(10f, 0f);
        textElement.Top.Set(2f, 0f); // 这是一个手动向下的偏移量

        Append(textElement);

        OnLeftClick += OnClick;
    }

    private void OnClick(UIMouseEvent evt, UIElement listeningElement)
    {
        parent.SetValue(value); // 点击选项立即通知配置修改
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        bool isSelected = parent.GetValue()?.Equals(value) == true;

        if (isSelected)
        {
            BackgroundColor = new Color(73, 94, 171);
            textElement.TextColor = Color.Yellow;
        }
        else if (IsMouseHovering)
        {
            BackgroundColor = new Color(73, 94, 171) * 0.7f;
            textElement.TextColor = Color.White;
        }
        else
        {
            BackgroundColor = new Color(63, 82, 151) * 0.7f;
            textElement.TextColor = Color.White;
        }
    }
}
