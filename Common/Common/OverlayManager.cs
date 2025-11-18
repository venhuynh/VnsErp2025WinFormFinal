using System;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraSplashScreen;

namespace Common.Common;

/// <summary>
/// Quản lý Overlay (DevExpress Overlay Form) để chặn thao tác và hiển thị trạng thái chờ.
/// - Hỗ trợ hiển thị trên Form hoặc Control bất kỳ.
/// - Cung cấp API dạng scope (IDisposable) để tự động đóng khi xong việc.
/// Tham khảo: DevExpress Overlay Form.
/// </summary>
public static class OverlayManager
{
    #region Business Methods

    /// <summary>
    /// Hiển thị Overlay trên một <see cref="Control"/> hoặc <see cref="Form"/>.
    /// </summary>
    /// <param name="owner">Control/Form cần phủ</param>
    /// <param name="options">Tùy chọn Overlay (không bắt buộc)</param>
    /// <returns>Handle của Overlay để đóng sau khi hoàn thành</returns>
    public static IOverlaySplashScreenHandle ShowOverlay(Control owner, OverlayWindowOptions options = null)
    {
        if (owner == null) throw new ArgumentNullException(nameof(owner));

        // Đảm bảo control đã tạo handle, nếu chưa sẽ gây InvalidOperationException theo DevExpress
        if (!owner.IsHandleCreated)
        {
            var handle = owner.Handle; // buộc tạo handle
        }

        return options == null
            ? SplashScreenManager.ShowOverlayForm(owner)
            : SplashScreenManager.ShowOverlayForm(
                owner: owner,
                startupDelay: options.StartupDelay,
                backColor: options.BackColor,
                opacity: options.Opacity,
                fadeIn: options.FadeIn,
                fadeOut: options.FadeOut,
                imageSize: options.ImageSize,
                customPainter: options.CustomPainter,
                disableInput: options.DisableInput,
                skinName: options.SkinName,
                animationType: options.AnimationType,
                image: options.Image,
                useDirectX: options.UseDirectX
            );
    }

    /// <summary>
    /// Đóng Overlay theo handle.
    /// </summary>
    /// <param name="handle">Handle trả về khi gọi <see cref="ShowOverlay"/></param>
    public static void CloseOverlay(IOverlaySplashScreenHandle handle)
    {
        if (handle == null) return;
        SplashScreenManager.CloseOverlayForm(handle);
    }

    /// <summary>
    /// Hiển thị Overlay theo dạng scope (sử dụng với <c>using</c> để tự đóng).
    /// </summary>
    /// <param name="owner">Control/Form cần phủ</param>
    /// <param name="options">Tùy chọn Overlay</param>
    /// <param name="configure">Tác vụ cấu hình handle (ví dụ QueueFocus hoặc QueueCloseUpAction)</param>
    /// <returns>IDisposable tự động đóng Overlay</returns>
    public static IDisposable ShowScope(Control owner, OverlayWindowOptions options = null, Action<IOverlaySplashScreenHandle> configure = null)
    {
        var handle = ShowOverlay(owner, options);
        configure?.Invoke(handle);
        return new OverlayScope(handle);
    }

    /// <summary>
    /// Hiển thị Overlay toàn Form chính (tiện lợi cho chặn toàn màn hình ứng dụng).
    /// </summary>
    /// <param name="form">Form top-level</param>
    /// <param name="message">Tùy chọn: nội dung hiển thị (cần painter tùy biến để render text)</param>
    /// <returns>Scope tự đóng</returns>
    public static IDisposable ShowMainFormScope(Form form, string message = null)
    {
        var opts = OverlayWindowOptions.Default;
        // Có thể tinh chỉnh hiệu ứng/fade nếu cần
        if (!string.IsNullOrWhiteSpace(message))
        {
            // Gợi ý: Để hiển thị message tùy biến, người dùng có thể truyền CustomPainter vào options
            // ở API ShowScope. Ở đây giữ đơn giản: dùng default indicator.
        }
        return ShowScope(form, opts);
    }

    #endregion

    #region Nested Types

    /// <summary>
    /// Scope disposable để tự động đóng Overlay khi Dispose.
    /// </summary>
    private sealed class OverlayScope : IDisposable
    {
        private IOverlaySplashScreenHandle _handle;
        private bool _disposed;

        public OverlayScope(IOverlaySplashScreenHandle handle)
        {
            _handle = handle;
        }

        public void Dispose()
        {
            if (_disposed) return;
            try
            {
                if (_handle != null)
                {
                    SplashScreenManager.CloseOverlayForm(_handle);
                    _handle = null;
                }
            }
            finally
            {
                _disposed = true;
            }
        }
    }

    #endregion
}

/// <summary>
/// Tập hợp tùy chọn hiển thị Overlay (map các tham số phổ biến từ DevExpress SplashScreenManager.ShowOverlayForm).
/// Lưu ý: Tất cả là tùy chọn; có thể dùng <see cref="Default"/> để dùng giá trị mặc định.
/// </summary>
public sealed class OverlayWindowOptions
{
    #region Fields & Properties

    public static OverlayWindowOptions Default => new OverlayWindowOptions();

    public int startupDelay;
    public System.Drawing.Color? backColor;
    public byte? opacity;
    public bool? fadeIn;
    public bool? fadeOut;
    public Size? imageSize;
    public DevExpress.XtraSplashScreen.OverlayWindowPainterBase customPainter;
    public bool? disableInput;
    public string skinName;
    public DevExpress.XtraSplashScreen.WaitAnimationType? animationType;
    public Image image;
    public bool? useDirectX;

    #endregion

    #region Accessors (convert to nullable-safe values)

    internal int? StartupDelay => startupDelay > 0 ? startupDelay : (int?)null;
    internal Color? BackColor => backColor;
    internal byte? Opacity => opacity;
    internal bool? FadeIn => fadeIn;
    internal bool? FadeOut => fadeOut;
    internal Size? ImageSize => imageSize;
    internal DevExpress.XtraSplashScreen.OverlayWindowPainterBase CustomPainter => customPainter;
    internal bool? DisableInput => disableInput;
    internal string SkinName => skinName;
    internal DevExpress.XtraSplashScreen.WaitAnimationType? AnimationType => animationType;
    internal Image Image => image;
    internal bool? UseDirectX => useDirectX;

    #endregion
}