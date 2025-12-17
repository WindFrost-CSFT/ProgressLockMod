using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.Config;
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

    protected override void SetObject(object value)
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

    private void UpdateHeaderText()
    {
        

        var currentValue = GetValue();
        if (currentValue != null)
        {
            headerText.SetText(currentValue.ToString());
        }
        else
        {
            headerText.SetText("点击选择...");
        }
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
        this.displayName = value.ToString();

        Width.Set(0f, 1f);
        Height.Set(28f, 0f);
        PaddingTop = 5f;
        PaddingLeft = 10f;

        BackgroundColor = new Color(63, 82, 151) * 0.7f;
        BorderColor = new Color(89, 116, 213) * 0.5f;

        textElement = new UIText(displayName);
        textElement.VAlign = 0f;
        textElement.VAlign = 0f;
        textElement.Left.Set(0f, 0f);
        textElement.Top.Set(0f, 0f);
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
