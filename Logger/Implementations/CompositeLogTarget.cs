using Logger.Interfaces;
using Logger.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logger.Implementations;

/// <summary>
/// Target logging kết hợp nhiều target
/// </summary>
public class CompositeLogTarget : ILogTarget
{
    #region Fields

    private readonly List<ILogTarget> _targets;
    private readonly object _lockObject = new object();

    #endregion

    #region Properties

    public string Name => "CompositeTarget";
    public bool IsEnabled { get; set; } = true;

    #endregion

    #region Constructor

    public CompositeLogTarget()
    {
        _targets = new List<ILogTarget>();
    }

    public CompositeLogTarget(params ILogTarget[] targets)
    {
        _targets = new List<ILogTarget>(targets ?? new ILogTarget[0]);
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Thêm target vào composite
    /// </summary>
    public void AddTarget(ILogTarget target)
    {
        if (target == null)
            throw new ArgumentNullException(nameof(target));

        lock (_lockObject)
        {
            if (!_targets.Contains(target))
            {
                _targets.Add(target);
            }
        }
    }

    /// <summary>
    /// Xóa target khỏi composite
    /// </summary>
    public void RemoveTarget(ILogTarget target)
    {
        if (target == null)
            return;

        lock (_lockObject)
        {
            _targets.Remove(target);
        }
    }

    /// <summary>
    /// Lấy tất cả targets
    /// </summary>
    public IReadOnlyList<ILogTarget> GetTargets()
    {
        lock (_lockObject)
        {
            return _targets.ToList().AsReadOnly();
        }
    }

    /// <summary>
    /// Ghi log entry đến tất cả targets
    /// </summary>
    public void Write(LogEntry entry)
    {
        if (!IsEnabled)
            return;

        lock (_lockObject)
        {
            foreach (var target in _targets.ToList())
            {
                try
                {
                    if (target.IsEnabled)
                    {
                        target.Write(entry);
                    }
                }
                catch (Exception ex)
                {
                    // Log error but don't let it stop other targets
                    System.Diagnostics.Debug.WriteLine($"Error writing to target {target.Name}: {ex.Message}");
                }
            }
        }
    }

    /// <summary>
    /// Flush tất cả targets
    /// </summary>
    public void Flush()
    {
        lock (_lockObject)
        {
            foreach (var target in _targets.ToList())
            {
                try
                {
                    if (target.IsEnabled)
                    {
                        target.Flush();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error flushing target {target.Name}: {ex.Message}");
                }
            }
        }
    }

    /// <summary>
    /// Đóng tất cả targets
    /// </summary>
    public void Close()
    {
        lock (_lockObject)
        {
            foreach (var target in _targets.ToList())
            {
                try
                {
                    target.Close();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error closing target {target.Name}: {ex.Message}");
                }
            }
        }
    }

    #endregion
}