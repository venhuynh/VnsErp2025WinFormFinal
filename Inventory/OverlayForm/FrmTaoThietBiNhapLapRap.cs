namespace Inventory.OverlayForm
{
    public partial class FrmTaoThietBiNhapLapRap : DevExpress.XtraEditors.XtraForm
    {
        //#region ========== FIELDS & PROPERTIES ==========

        ///// <summary>
        ///// Business Logic Layer cho phiếu nhập xuất kho
        ///// </summary>
        //private readonly StockInOutMasterBll _stockInOutMasterBll = new StockInOutMasterBll();

        ///// <summary>
        ///// Business Logic Layer cho chi tiết nhập xuất kho
        ///// </summary>
        //private readonly StockInOutBll _stockInOutBll = new StockInOutBll();

        ///// <summary>
        ///// Business Logic Layer cho sản phẩm/dịch vụ
        ///// </summary>
        //private readonly ProductServiceBll _productServiceBll = new ProductServiceBll();

        ///// <summary>
        ///// Business Logic Layer cho biến thể sản phẩm
        ///// </summary>
        //private readonly ProductVariantBll _productVariantBll = new ProductVariantBll();

        ///// <summary>
        ///// Business Logic Layer cho đơn vị tính
        ///// </summary>
        //private readonly UnitOfMeasureBll _unitOfMeasureBll = new UnitOfMeasureBll();

        ///// <summary>
        ///// Business Logic Layer cho thuộc tính
        ///// </summary>
        //private readonly AttributeBll _attributeBll = new AttributeBll();

        ///// <summary>
        ///// Logger để ghi log các sự kiện
        ///// </summary>
        //private readonly ILogger _logger = LoggerFactory.CreateLogger(LogCategory.UI);

        //#endregion

        //#region ========== CONSTRUCTOR ==========

        //public FrmTaoThietBiNhapLapRap()
        //{
        //    InitializeComponent();
        //    Load += FrmTaoThietBiNhapLapRap_Load;
        //    PhieuXuatLapRapLookupEdit.EditValueChanged += PhieuXuatLapRapLookupEdit_EditValueChanged;
        //    SaveBarButtonItem.ItemClick += SaveBarButtonItem_ItemClick;
        //}

        //#endregion

        //#region ========== EVENT HANDLERS ==========

        ///// <summary>
        ///// Event handler khi form được load
        ///// </summary>
        //private async void FrmTaoThietBiNhapLapRap_Load(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        // Hiển thị SplashScreen một lần cho cả 3 operations
        //        SplashScreenHelper.ShowWaitingSplashScreen();
        //        try
        //        {
        //            // Load cả 3 datasource song song
        //            await Task.WhenAll(
        //                LoadNhapLapRapListAsyncWithoutSplash(),
        //                LoadProductServiceListAsyncWithoutSplash(),
        //                LoadUnitOfMeasureListAsyncWithoutSplash()
        //            );
        //        }
        //        finally
        //        {
        //            SplashScreenHelper.CloseSplashScreen();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error($"FrmTaoThietBiNhapLapRap_Load: Lỗi load dữ liệu: {ex.Message}", ex);
        //        SplashScreenHelper.CloseSplashScreen();
        //        MsgBox.ShowError($"Lỗi load dữ liệu: {ex.Message}");
        //    }
        //}

        ///// <summary>
        ///// Event handler khi giá trị của PhieuXuatLapRapLookupEdit thay đổi
        ///// Load thông tin ProductVariant và số lượng từ phiếu xuất lắp ráp và hiển thị vào VariantAttributeDtoGridControl
        ///// </summary>
        //private async void PhieuXuatLapRapLookupEdit_EditValueChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        // Lấy master id từ lookup edit
        //        var masterId = GetMasterIdFromEditValue();
                
        //        if (masterId == null)
        //        {
        //            _logger.Debug("PhieuXuatLapRapLookupEdit_EditValueChanged: EditValue is null or invalid, clearing grid");
        //            ClearVariantAttributeGrid();
        //            return;
        //        }

        //        if (masterId == Guid.Empty)
        //        {
        //            _logger.Debug("PhieuXuatLapRapLookupEdit_EditValueChanged: MasterId is Guid.Empty, clearing grid");
        //            ClearVariantAttributeGrid();
        //            return;
        //        }

        //        _logger.Debug($"PhieuXuatLapRapLookupEdit_EditValueChanged: Bắt đầu load detail cho MasterId={masterId}");

        //        // Load dữ liệu với splash screen
        //        await LoadConfigurationDataAsync(masterId.Value);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error($"PhieuXuatLapRapLookupEdit_EditValueChanged: Lỗi xử lý sự kiện: {ex.Message}", ex);
        //        SplashScreenHelper.CloseSplashScreen();
        //        MsgBox.ShowError($"Lỗi xử lý sự kiện: {ex.Message}");
        //        ClearVariantAttributeGrid();
        //    }
        //}

        ///// <summary>
        ///// Lấy MasterId từ EditValue của PhieuXuatLapRapLookupEdit
        ///// </summary>
        ///// <returns>Guid? - MasterId hoặc null nếu không hợp lệ</returns>
        //private Guid? GetMasterIdFromEditValue()
        //{
        //    try
        //    {
        //        var editValue = PhieuXuatLapRapLookupEdit.EditValue;
                
        //        if (editValue == null)
        //        {
        //            return null;
        //        }

        //        // Convert EditValue sang Guid
        //        if (editValue is Guid guidValue)
        //        {
        //            return guidValue;
        //        }
                
        //        if (editValue is string stringValue && Guid.TryParse(stringValue, out var parsedGuid))
        //        {
        //            return parsedGuid;
        //        }

        //        _logger.Warning($"GetMasterIdFromEditValue: Cannot convert EditValue to Guid. Type: {editValue.GetType()}, Value: {editValue}");
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error($"GetMasterIdFromEditValue: Lỗi xử lý: {ex.Message}", ex);
        //        return null;
        //    }
        //}

        ///// <summary>
        ///// Load dữ liệu cấu hình thiết bị từ phiếu xuất lắp ráp và hiển thị vào VariantAttributeDtoGridControl
        ///// </summary>
        ///// <param name="masterId">ID của phiếu xuất lắp ráp</param>
        //private async Task LoadConfigurationDataAsync(Guid masterId)
        //{
        //    try
        //    {
        //        SplashScreenHelper.ShowWaitingSplashScreen();
        //        try
        //        {
        //            await Task.Run(async () =>
        //            {
        //                try
        //                {
        //                    // Load chi tiết phiếu xuất lắp ráp
        //                    var detailDtos = await LoadStockOutDetailsAsync(masterId);
                            
        //                    if (detailDtos == null || detailDtos.Count == 0)
        //                    {
        //                        _logger.Warning($"LoadConfigurationDataAsync: Không có detail DTOs cho MasterId={masterId}");
        //                        BeginInvoke(new Action(() =>
        //                        {
        //                            ClearVariantAttributeGrid();
        //                        }));
        //                        return;
        //                    }

        //                    var variantAttributes = new List<VariantAttributeDto>();
        //                    foreach (var detailDto in detailDtos)
        //                    {
        //                        variantAttributes.Add(new VariantAttributeDto
        //                        {
        //                            VariantId = detailDto.ProductVariantId,
        //                            AttributeId = Guid.Empty,
        //                            AttributeValueId = Guid.Empty,
        //                            AttributeName = _productVariantBll.GetForNewAttribute(detailDto.ProductVariantId),
        //                            AttributeValue = detailDto.StockOutQty.ToString(CultureInfo.InvariantCulture) + " " + detailDto.UnitOfMeasureName
        //                        });
        //                    }

        //                    BeginInvoke(new Action(() =>
        //                    {
        //                        try
        //                        {
        //                            variantAttributeDtoBindingSource.DataSource = variantAttributes;
        //                            variantAttributeDtoBindingSource.ResetBindings(false);
        //                            _logger.Debug($"LoadConfigurationDataAsync: Đã cập nhật grid với {variantAttributes.Count} dòng");
        //                        }
        //                        catch (Exception uiEx)
        //                        {
        //                            _logger.Error($"LoadConfigurationDataAsync: Lỗi khi cập nhật grid: {uiEx.Message}", uiEx);
        //                            MsgBox.ShowError($"Lỗi hiển thị dữ liệu: {uiEx.Message}");
        //                        }
        //                    }));

        //                }
        //                catch (Exception ex)
        //                {
        //                    _logger.Error($"LoadConfigurationDataAsync: Exception occurred", ex);
        //                    BeginInvoke(new Action(() =>
        //                    {
        //                        MsgBox.ShowError($"Lỗi tải chi tiết phiếu xuất lắp ráp: {ex.Message}");
        //                        ClearVariantAttributeGrid();
        //                    }));
        //                }
        //            });
        //        }
        //        finally
        //        {
        //            SplashScreenHelper.CloseSplashScreen();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error($"LoadConfigurationDataAsync: Lỗi load dữ liệu cấu hình: {ex.Message}", ex);
        //        SplashScreenHelper.CloseSplashScreen();
        //        throw;
        //    }
        //}

        ///// <summary>
        ///// Load chi tiết phiếu xuất lắp ráp và enrich với thông tin ProductVariant
        ///// </summary>
        ///// <param name="masterId">ID của phiếu xuất lắp ráp</param>
        ///// <returns>Danh sách StockInOutDetailForUIDto đã được enrich</returns>
        //private async Task<List<StockInOutDetailForUIDto>> LoadStockOutDetailsAsync(Guid masterId)
        //{
        //    try
        //    {
        //        // Lấy detail DTOs từ BLL (BLL đã trả về DTOs)
        //        var detailDtos = await Task.Run(() => _stockInOutBll.GetStockInOutDetailsByMasterId(masterId));
        //        _logger.Debug($"LoadStockOutDetailsAsync: Đã lấy được {detailDtos.Count} detail DTOs");

        //        if (detailDtos.Count == 0)
        //        {
        //            return new List<StockInOutDetailForUIDto>();
        //        }
                
        //        // Lấy danh sách ProductVariantIds duy nhất
        //        var productVariantIds = detailDtos
        //            .Where(d => d.ProductVariantId != Guid.Empty)
        //            .Select(d => d.ProductVariantId)
        //            .Distinct()
        //            .ToList();

        //        // Load ProductVariant entities và enrich DTOs
        //        if (productVariantIds.Count > 0)
        //        {
        //            await EnrichDetailDtosWithProductVariantsAsync(detailDtos, productVariantIds);
        //        }

        //        return detailDtos;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error($"LoadStockOutDetailsAsync: Lỗi load chi tiết: {ex.Message}", ex);
        //        throw;
        //    }
        //}

        ///// <summary>
        ///// Enrich danh sách XuatLapRapDetailDto với thông tin ProductVariant
        ///// </summary>
        ///// <param name="detailDtos">Danh sách DTOs cần enrich</param>
        ///// <param name="productVariantIds">Danh sách ProductVariantIds cần load</param>
        //private async Task EnrichDetailDtosWithProductVariantsAsync(List<StockInOutDetailForUIDto> detailDtos, List<Guid> productVariantIds)
        //{
        //    try
        //    {
        //        _logger.Debug($"EnrichDetailDtosWithProductVariantsAsync: Bắt đầu load {productVariantIds.Count} ProductVariant entities");

        //        // Load ProductVariant DTOs song song để tối ưu hiệu suất
        //        var productVariantTasks = productVariantIds.Select(async variantId =>
        //        {
        //            try
        //            {
        //                // GetById đã trả về DTO, không cần convert
        //                var variantDto = await Task.Run(() => _productVariantBll.GetById(variantId));
        //                if (variantDto != null)
        //                {
        //                    return new { VariantId = variantId, VariantDto = variantDto };
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.Warning($"EnrichDetailDtosWithProductVariantsAsync: Không thể lấy ProductVariant với Id={variantId}: {ex.Message}");
        //            }
        //            return null;
        //        });

        //        var productVariantResults = await Task.WhenAll(productVariantTasks);
                
        //        // Tạo dictionary để lookup nhanh
        //        var productVariantDtoDict = productVariantResults
        //            .Where(r => r != null && r.VariantDto != null)
        //            .ToDictionary(r => r.VariantId, r => r.VariantDto);

        //        _logger.Debug($"EnrichDetailDtosWithProductVariantsAsync: Đã lấy được {productVariantDtoDict.Count} ProductVariantDto");

        //        // Enrich DTOs với thông tin ProductVariant
        //        foreach (var dto in detailDtos)
        //        {
        //            if (dto.ProductVariantId != Guid.Empty && 
        //                productVariantDtoDict.TryGetValue(dto.ProductVariantId, out var variantDto))
        //            {
        //                EnrichDetailDtoWithProductVariant(dto, variantDto);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error($"EnrichDetailDtosWithProductVariantsAsync: Lỗi enrich DTOs: {ex.Message}", ex);
        //        throw;
        //    }
        //}

        ///// <summary>
        ///// Enrich một XuatLapRapDetailDto với thông tin từ ProductVariantDto
        ///// </summary>
        ///// <param name="detailDto">DTO cần enrich</param>
        ///// <param name="variantDto">ProductVariantDto chứa thông tin</param>
        //private void EnrichDetailDtoWithProductVariant(StockInOutDetailForUIDto detailDto, ProductVariantDto variantDto)
        //{
        //    try
        //    {
        //        // Bổ sung ProductVariantCode nếu thiếu
        //        if (string.IsNullOrWhiteSpace(detailDto.ProductVariantCode) && !string.IsNullOrWhiteSpace(variantDto.VariantCode))
        //        {
        //            detailDto.ProductVariantCode = variantDto.VariantCode;
        //        }
                
        //        // Bổ sung ProductVariantName nếu thiếu
        //        if (string.IsNullOrWhiteSpace(detailDto.ProductVariantName))
        //        {
        //            var nameParts = new List<string>();
        //            if (!string.IsNullOrWhiteSpace(variantDto.VariantCode))
        //                nameParts.Add(variantDto.VariantCode);
        //            if (!string.IsNullOrWhiteSpace(variantDto.ProductName))
        //                nameParts.Add(variantDto.ProductName);
        //            if (!string.IsNullOrWhiteSpace(variantDto.VariantName))
        //                nameParts.Add(variantDto.VariantName);
                    
        //            if (nameParts.Count > 0)
        //            {
        //                detailDto.ProductVariantName = string.Join(" ", nameParts);
        //                if (!string.IsNullOrWhiteSpace(variantDto.UnitName))
        //                {
        //                    detailDto.ProductVariantName += $" ({variantDto.UnitName})";
        //                }
        //            }
        //        }
                
        //        // Bổ sung thông tin đơn vị tính nếu thiếu
        //        if (!detailDto.UnitOfMeasureId.HasValue && variantDto.UnitId != Guid.Empty)
        //        {
        //            detailDto.UnitOfMeasureId = variantDto.UnitId;
        //        }
                
        //        if (string.IsNullOrWhiteSpace(detailDto.UnitOfMeasureCode) && !string.IsNullOrWhiteSpace(variantDto.UnitCode))
        //        {
        //            detailDto.UnitOfMeasureCode = variantDto.UnitCode;
        //        }
                
        //        if (string.IsNullOrWhiteSpace(detailDto.UnitOfMeasureName) && !string.IsNullOrWhiteSpace(variantDto.UnitName))
        //        {
        //            detailDto.UnitOfMeasureName = variantDto.UnitName;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Warning($"EnrichDetailDtoWithProductVariant: Lỗi enrich DTO: {ex.Message}");
        //        // Không throw để tiếp tục với các DTO khác
        //    }
        //}

        ///// <summary>
        ///// Clear dữ liệu trong VariantAttributeDtoGridControl
        ///// </summary>
        //private void ClearVariantAttributeGrid()
        //{
        //    try
        //    {
        //        variantAttributeDtoBindingSource.DataSource = null;
        //        variantAttributeDtoBindingSource.ResetBindings(false);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Warning($"ClearVariantAttributeGrid: Lỗi clear grid: {ex.Message}");
        //    }
        //}

        ///// <summary>
        ///// Format danh sách XuatLapRapDetailDto thành text để hiển thị (deprecated - không còn sử dụng)
        ///// </summary>
        ///// <param name="detailDtos">Danh sách chi tiết phiếu xuất lắp ráp</param>
        ///// <returns>Text đã được format</returns>
        //[Obsolete("Method này không còn được sử dụng, dữ liệu hiện được hiển thị trong grid")]
        //private string FormatConfigurationText(List<StockInOutDetailForUIDto> detailDtos)
        //{
        //    if (detailDtos == null || detailDtos.Count == 0)
        //    {
        //        return string.Empty;
        //    }

        //    var lines = new List<string>();
            
        //    for (int i = 0; i < detailDtos.Count; i++)
        //    {
        //        var dto = detailDtos[i];
        //        var lineNumber = i + 1;
                
        //        // Tên ProductVariant
        //        var variantName = !string.IsNullOrWhiteSpace(dto.ProductVariantName) 
        //            ? dto.ProductVariantName.Trim()
        //            : !string.IsNullOrWhiteSpace(dto.ProductVariantCode)
        //                ? dto.ProductVariantCode.Trim()
        //                : "Không xác định";
                
        //        // Loại bỏ tất cả HTML tags và chuyển <br> thành xuống hàng
        //        variantName = StripHtmlTags(variantName);
                
        //        // Tách variantName thành các dòng nếu có newline
        //        var variantNameLines = variantName.Split(new[] { Environment.NewLine, "\n", "\r\n" }, StringSplitOptions.None);
                
        //        // Format: STT. Tên ProductVariant - Số lượng [Đơn vị]
        //        // Nếu variantName có nhiều dòng, mỗi dòng sẽ được format riêng
        //        for (int j = 0; j < variantNameLines.Length; j++)
        //        {
        //            var variantNameLine = variantNameLines[j].Trim();
        //            if (string.IsNullOrWhiteSpace(variantNameLine))
        //                continue;
                    
        //            var lineParts = new List<string>();
                    
        //            // Số thứ tự chỉ hiển thị ở dòng đầu tiên
        //            if (j == 0)
        //            {
        //                lineParts.Add($"{lineNumber}.");
        //            }
        //            else
        //            {
        //                // Các dòng tiếp theo có indent
        //                lineParts.Add(new string(' ', $"{lineNumber}.".Length + 1));
        //            }
                    
        //            lineParts.Add(variantNameLine);
                    
        //            // Số lượng và đơn vị chỉ hiển thị ở dòng cuối cùng
        //            if (j == variantNameLines.Length - 1)
        //            {
        //                if (dto.StockOutQty > 0)
        //                {
        //                    lineParts.Add($"- {dto.StockOutQty:N2}");
                            
        //                    // Đơn vị tính
        //                    if (!string.IsNullOrWhiteSpace(dto.UnitOfMeasureName))
        //                    {
        //                        lineParts.Add($"[{dto.UnitOfMeasureName}]");
        //                    }
        //                    else if (!string.IsNullOrWhiteSpace(dto.UnitOfMeasureCode))
        //                    {
        //                        lineParts.Add($"[{dto.UnitOfMeasureCode}]");
        //                    }
        //                }
        //            }
                    
        //            lines.Add(string.Join(" ", lineParts));
        //        }
        //    }
            
        //    return string.Join(Environment.NewLine, lines);
        //}

        ///// <summary>
        ///// Loại bỏ tất cả HTML tags và chuyển <br> tags thành xuống hàng
        ///// </summary>
        ///// <param name="htmlText">Text chứa HTML tags</param>
        ///// <returns>Text đã được loại bỏ HTML tags, <br> được chuyển thành newline</returns>
        //private string StripHtmlTags(string htmlText)
        //{
        //    if (string.IsNullOrWhiteSpace(htmlText))
        //    {
        //        return htmlText;
        //    }

        //    try
        //    {
        //        // Thay thế các thẻ <br> và <br/> bằng newline trước khi loại bỏ HTML tags
        //        // Xử lý cả <br>, <br/>, <br />, <BR>, etc. (case-insensitive)
        //        // Sử dụng placeholder tạm thời để bảo vệ newline
        //        htmlText = Regex.Replace(htmlText, @"<br\s*/?>", 
        //            Environment.NewLine, RegexOptions.IgnoreCase);
                
        //        // Loại bỏ tất cả các HTML tags còn lại
        //        // Pattern này sẽ match: <tag>, </tag>, <tag attribute="value">, <tag/>
        //        htmlText = Regex.Replace(htmlText, @"<[^>]+>", string.Empty);
                
        //        // Decode các HTML entities phổ biến
        //        htmlText = htmlText.Replace("&nbsp;", " ");
        //        htmlText = htmlText.Replace("&amp;", "&");
        //        htmlText = htmlText.Replace("&lt;", "<");
        //        htmlText = htmlText.Replace("&gt;", ">");
        //        htmlText = htmlText.Replace("&quot;", "\"");
        //        htmlText = htmlText.Replace("&#39;", "'");
                
        //        // Loại bỏ các khoảng trắng thừa TRƯỚC newline (giữ lại newline)
        //        // Thay thế nhiều khoảng trắng liên tiếp bằng một khoảng trắng, nhưng không thay thế newline
        //        htmlText = Regex.Replace(htmlText, @"[ \t]+", " ");
                
        //        // Loại bỏ khoảng trắng ở đầu và cuối mỗi dòng (nhưng giữ lại newline)
        //        htmlText = Regex.Replace(htmlText, @"[ \t]*\r\n[ \t]*", Environment.NewLine);
        //        htmlText = Regex.Replace(htmlText, @"[ \t]*\n[ \t]*", Environment.NewLine);
                
        //        // Loại bỏ nhiều newline liên tiếp (giữ lại tối đa 2 newline)
        //        htmlText = Regex.Replace(htmlText, @"(\r\n\s*){3,}", Environment.NewLine + Environment.NewLine);
        //        htmlText = Regex.Replace(htmlText, @"(\n\s*){3,}", Environment.NewLine + Environment.NewLine);
                
        //        return htmlText.Trim();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Warning($"StripHtmlTags: Lỗi xử lý HTML: {ex.Message}");
        //        // Nếu có lỗi, trả về text gốc
        //        return htmlText;
        //    }
        //}

        //#endregion

        //#region ========== DATA LOADING ==========

        ///// <summary>
        ///// Load danh sách các phiếu nhập lắp ráp theo thứ tự ngày nhập mới nhất (không hiển thị SplashScreen)
        ///// </summary>
        //private async Task LoadNhapLapRapListAsyncWithoutSplash()
        //{
        //    try
        //    {
        //        _logger.Debug("LoadNhapLapRapListAsyncWithoutSplash: Bắt đầu load danh sách phiếu nhập lắp ráp");

        //        await Task.Run(() =>
        //        {
        //            try
        //            {
        //                // Lấy dữ liệu từ BLL (trả về entities)
        //                var entities = _stockInOutMasterBll.GetPhieuNhapLapRap();

        //                // Map entities sang StockInOutMasterForUIDto, sau đó convert sang NhapLapRapMasterListDto
        //                var masterDtos = entities.ToDtoList();
                        
        //                // Convert sang NhapLapRapMasterListDto
        //                var dtos = masterDtos.Select(m => new DTO.Inventory.StockIn.NhapLapRap.NhapLapRapMasterListDto
        //                {
        //                    Id = m.Id,
        //                    VoucherNumber = m.VoucherNumber,
        //                    StockInDate = m.StockOutDate, // Note: StockOutDate trong DTO được dùng cho cả nhập và xuất
        //                    LoaiNhapXuatKho = m.LoaiNhapXuatKho,
        //                    TrangThai = m.TrangThai,
        //                    WarehouseId = m.WarehouseId,
        //                    WarehouseCode = m.WarehouseCode,
        //                    WarehouseName = m.WarehouseName,
        //                    SupplierId = m.CustomerId, // Note: CustomerId được dùng cho Supplier trong trường hợp nhập
        //                    SupplierName = m.CustomerName, // Note: CustomerName được dùng cho SupplierName trong trường hợp nhập
        //                    Notes = m.Notes,
        //                    TotalQuantity = m.TotalQuantity,
        //                    TotalAmount = m.TotalAmount,
        //                    TotalVat = m.TotalVat,
        //                    TotalAmountIncludedVat = m.TotalAmountIncludedVat
        //                }).ToList();

        //                // Update UI thread
        //                BeginInvoke(new Action(() =>
        //                {
        //                    xuatLapRapMasterListDtoBindingSource.DataSource = dtos;
        //                    xuatLapRapMasterListDtoBindingSource.ResetBindings(false);
        //                }));

        //                _logger.Info($"LoadNhapLapRapListAsyncWithoutSplash: Đã load {dtos.Count} phiếu nhập lắp ráp");
        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.Error("LoadNhapLapRapListAsyncWithoutSplash: Exception occurred", ex);
        //                BeginInvoke(new Action(() =>
        //                {
        //                    MsgBox.ShowError($"Lỗi tải danh sách phiếu nhập lắp ráp: {ex.Message}");
        //                }));
        //            }
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error($"LoadNhapLapRapListAsyncWithoutSplash: Lỗi load danh sách phiếu nhập lắp ráp: {ex.Message}", ex);
        //        throw;
        //    }
        //}

        ///// <summary>
        ///// Load danh sách sản phẩm/dịch vụ tương tự như FrmProductServiceList (không hiển thị SplashScreen)
        ///// </summary>
        //private async Task LoadProductServiceListAsyncWithoutSplash()
        //{
        //    try
        //    {
        //        _logger.Debug("LoadProductServiceListAsyncWithoutSplash: Bắt đầu load danh sách sản phẩm/dịch vụ");

        //        // Get all data (BLL đã trả về DTOs)
        //        var dtoList = await _productServiceBll.GetAllAsync();
        //        _logger.Debug($"LoadProductServiceListAsyncWithoutSplash: Đã nhận được {dtoList?.Count ?? 0} DTOs từ BLL");

        //        // Load tất cả categories một lần vào dictionary để tối ưu hiệu suất (nếu cần enrich)
        //        var categoryDict = await _productServiceBll.GetCategoryDictAsync();
        //        _logger.Debug($"LoadProductServiceListAsyncWithoutSplash: Đã lấy được {categoryDict?.Count ?? 0} categories");
        //        _logger.Debug($"LoadProductServiceListAsyncWithoutSplash: Đã convert được {dtoList.Count} DTOs");

        //        // Update UI thread
        //        BeginInvoke(new Action(() =>
        //        {
        //            productServiceDtoBindingSource.DataSource = dtoList;
        //            productServiceDtoBindingSource.ResetBindings(false);
        //        }));

        //        _logger.Info($"LoadProductServiceListAsyncWithoutSplash: Đã load {dtoList.Count} sản phẩm/dịch vụ");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error($"LoadProductServiceListAsyncWithoutSplash: Lỗi load danh sách sản phẩm/dịch vụ: {ex.Message}", ex);
        //        BeginInvoke(new Action(() =>
        //        {
        //            MsgBox.ShowError($"Lỗi tải danh sách sản phẩm/dịch vụ: {ex.Message}");
        //        }));
        //        throw;
        //    }
        //}

        ///// <summary>
        ///// Load danh sách đơn vị tính (không hiển thị SplashScreen)
        ///// </summary>
        //private async Task LoadUnitOfMeasureListAsyncWithoutSplash()
        //{
        //    try
        //    {
        //        _logger.Debug("LoadUnitOfMeasureListAsyncWithoutSplash: Bắt đầu load danh sách đơn vị tính");

        //        await Task.Run(() =>
        //        {
        //            try
        //            {
        //                // Lấy tất cả đơn vị tính đang hoạt động (BLL đã trả về DTOs)
        //                var dtoList = _unitOfMeasureBll.GetByStatus(true)
        //                    .OrderBy(u => u.Name)
        //                    .ToList();
        //                _logger.Debug($"LoadUnitOfMeasureListAsyncWithoutSplash: Đã nhận được {dtoList?.Count ?? 0} DTOs từ BLL");
        //                _logger.Debug($"LoadUnitOfMeasureListAsyncWithoutSplash: Đã convert được {dtoList.Count} DTOs");

        //                // Update UI thread
        //                BeginInvoke(new Action(() =>
        //                {
        //                    unitOfMeasureDtoBindingSource.DataSource = dtoList;
        //                    unitOfMeasureDtoBindingSource.ResetBindings(false);
        //                }));

        //                _logger.Info($"LoadUnitOfMeasureListAsyncWithoutSplash: Đã load {dtoList.Count} đơn vị tính");
        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.Error("LoadUnitOfMeasureListAsyncWithoutSplash: Exception occurred", ex);
        //                BeginInvoke(new Action(() =>
        //                {
        //                    MsgBox.ShowError($"Lỗi tải danh sách đơn vị tính: {ex.Message}");
        //                }));
        //            }
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error($"LoadUnitOfMeasureListAsyncWithoutSplash: Lỗi load danh sách đơn vị tính: {ex.Message}", ex);
        //        throw;
        //    }
        //}


        //#endregion

        //#region ========== SAVE OPERATIONS ==========

        ///// <summary>
        ///// Event handler khi người dùng bấm nút Lưu
        ///// </summary>
        //private async void SaveBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    try
        //    {
        //        // Validate dữ liệu
        //        ValidateBeforeSave();

        //        // Hiển thị SplashScreen
        //        SplashScreenHelper.ShowWaitingSplashScreen();

        //        try
        //        {
        //            await Task.Run(async () =>
        //            {
        //                try
        //                {
        //                    // Lấy dữ liệu từ form
        //                    var productId = GetProductId();
        //                    var variantCode = GetVariantCode();
        //                    var variantFullName = GetVariantFullName();
        //                    var isActive = GetIsActive();
        //                    var unitId = GetUnitId();
        //                    var attributeValues = GetAttributeValues();

        //                    // Kiểm tra trùng lặp ProductId và VariantFullName
        //                    await CheckDuplicateAsync(productId, variantFullName);

        //                    // Tạo ProductVariantDto
        //                    var variant = new ProductVariantDto
        //                    {
        //                        Id = Guid.NewGuid(),
        //                        ProductId = productId,
        //                        VariantCode = variantCode,
        //                        UnitId = unitId,
        //                        IsActive = isActive
        //                    };

        //                    // Lưu ProductVariant (SaveAsync sẽ tự động tạo VariantFullName)
        //                    var savedVariantId = await _productVariantBll.SaveAsync(variant, attributeValues);

        //                    // Update UI thread
        //                    BeginInvoke(new Action(() =>
        //                    {
        //                        MsgBox.ShowSuccess("Lưu biến thể sản phẩm thành công!");
        //                        DialogResult = DialogResult.OK;
        //                        Close();
        //                    }));

        //                    _logger.Info($"SaveBarButtonItem_ItemClick: Đã lưu ProductVariant thành công, Id={savedVariantId}");
        //                }
        //                catch (Exception ex)
        //                {
        //                    _logger.Error($"SaveBarButtonItem_ItemClick: Exception occurred", ex);
        //                    BeginInvoke(new Action(() =>
        //                    {
        //                        MsgBox.ShowError($"Lỗi lưu biến thể sản phẩm: {ex.Message}");
        //                    }));
        //                }
        //            });
        //        }
        //        finally
        //        {
        //            SplashScreenHelper.CloseSplashScreen();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error($"SaveBarButtonItem_ItemClick: Lỗi xử lý sự kiện: {ex.Message}", ex);
        //        SplashScreenHelper.CloseSplashScreen();
        //        MsgBox.ShowError($"Lỗi xử lý sự kiện: {ex.Message}");
        //    }
        //}

        ///// <summary>
        ///// Validate dữ liệu trước khi lưu
        ///// </summary>
        //private void ValidateBeforeSave()
        //{
        //    // 1. Phiếu nhập lắp ráp không được để trống
        //    if (PhieuXuatLapRapLookupEdit.EditValue == null || 
        //        !(PhieuXuatLapRapLookupEdit.EditValue is Guid) || 
        //        (Guid)PhieuXuatLapRapLookupEdit.EditValue == Guid.Empty)
        //    {
        //        throw new ArgumentException("Vui lòng chọn phiếu nhập lắp ráp.");
        //    }

        //    // 2. Sản phẩm không được để trống
        //    if (ProductServiceLookupEdit.EditValue == null || 
        //        !(ProductServiceLookupEdit.EditValue is Guid) || 
        //        (Guid)ProductServiceLookupEdit.EditValue == Guid.Empty)
        //    {
        //        throw new ArgumentException("Vui lòng chọn sản phẩm.");
        //    }

        //    // 3. Đơn vị tính không được để trống
        //    if (UnitOfMeasureSearchLookupEdit.EditValue == null || 
        //        !(UnitOfMeasureSearchLookupEdit.EditValue is Guid) || 
        //        (Guid)UnitOfMeasureSearchLookupEdit.EditValue == Guid.Empty)
        //    {
        //        throw new ArgumentException("Vui lòng chọn đơn vị tính.");
        //    }
        //}

        ///// <summary>
        ///// Lấy ProductId từ ProductServiceLookupEdit
        ///// </summary>
        //private Guid GetProductId()
        //{
        //    if (ProductServiceLookupEdit.EditValue is Guid productId)
        //    {
        //        return productId;
        //    }
        //    throw new InvalidOperationException("Không thể lấy ProductId từ ProductServiceLookupEdit.");
        //}

        ///// <summary>
        ///// Tạo VariantCode tự động từ ProductId và timestamp
        ///// </summary>
        //private string GetVariantCode()
        //{
        //    var productId = GetProductId();
        //    // Tạo mã biến thể tự động: "VAR" + ProductId (8 ký tự đầu) + timestamp
        //    var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        //    var productIdShort = productId.ToString("N").Substring(0, 8).ToUpper();
        //    return $"VAR{productIdShort}{timestamp}";
        //}

        ///// <summary>
        ///// Lấy VariantFullName (tạm thời trả về empty string, cần implement logic mới)
        ///// </summary>
        //private string GetVariantFullName()
        //{
        //    // TODO: Implement logic mới để lấy VariantFullName
        //    return string.Empty;
        //}

        ///// <summary>
        ///// Lấy IsActive (mặc định là true)
        ///// </summary>
        //private bool GetIsActive()
        //{
        //    return true; // Mặc định là true khi tạo mới
        //}

        ///// <summary>
        ///// Lấy UnitId từ UnitOfMeasureSearchLookupEdit
        ///// </summary>
        //private Guid GetUnitId()
        //{
        //    if (UnitOfMeasureSearchLookupEdit.EditValue is Guid unitId)
        //    {
        //        return unitId;
        //    }
        //    throw new ArgumentException("Vui lòng chọn đơn vị tính.");
        //}

        ///// <summary>
        ///// Lấy danh sách AttributeValues từ variantAttributeDtoBindingSource
        ///// Validate AttributeName không trùng lặp và tìm/tạo AttributeId
        ///// </summary>
        //private List<(Guid AttributeId, string Value)> GetAttributeValues()
        //{
        //    var result = new List<(Guid AttributeId, string Value)>();
            
        //    // Lấy dữ liệu từ binding source
        //    if (variantAttributeDtoBindingSource.DataSource == null)
        //    {
        //        return result;
        //    }

        //    var variantAttributeDtos = variantAttributeDtoBindingSource.DataSource as List<VariantAttributeDto>;
        //    if (variantAttributeDtos == null)
        //    {
        //        // Nếu không phải List, thử cast từ IEnumerable
        //        var enumerable = variantAttributeDtoBindingSource.DataSource as System.Collections.IEnumerable;
        //        if (enumerable != null)
        //        {
        //            variantAttributeDtos = enumerable.Cast<VariantAttributeDto>().ToList();
        //        }
        //        else
        //        {
        //            return result;
        //        }
        //    }

        //    if (variantAttributeDtos.Count == 0)
        //    {
        //        return result;
        //    }

        //    // Validate AttributeName không trùng lặp
        //    ValidateAttributeNamesNotDuplicate(variantAttributeDtos);

        //    // Convert VariantAttributeDto thành List<(Guid AttributeId, string Value)>
        //    foreach (var dto in variantAttributeDtos)
        //    {
        //        if (string.IsNullOrWhiteSpace(dto.AttributeName))
        //        {
        //            continue; // Bỏ qua nếu AttributeName rỗng
        //        }

        //        // Tìm hoặc tạo Attribute dựa trên AttributeName
        //        var attributeId = GetOrCreateAttributeId(dto.AttributeName);
                
        //        if (attributeId != Guid.Empty)
        //        {
        //            result.Add((attributeId, dto.AttributeValue ?? string.Empty));
        //        }
        //    }

        //    return result;
        //}

        ///// <summary>
        ///// Validate AttributeName không trùng lặp trong danh sách
        ///// </summary>
        //private void ValidateAttributeNamesNotDuplicate(List<VariantAttributeDto> variantAttributeDtos)
        //{
        //    var attributeNameGroups = variantAttributeDtos
        //        .Where(dto => !string.IsNullOrWhiteSpace(dto.AttributeName))
        //        .GroupBy(dto => dto.AttributeName.Trim(), StringComparer.OrdinalIgnoreCase)
        //        .Where(g => g.Count() > 1)
        //        .ToList();

        //    if (attributeNameGroups.Any())
        //    {
        //        var duplicateNames = string.Join(", ", attributeNameGroups.Select(g => $"'{g.Key}'"));
        //        throw new ArgumentException($"Tên thuộc tính bị trùng lặp: {duplicateNames}. Vui lòng xóa các thuộc tính trùng lặp.");
        //    }
        //}

        ///// <summary>
        ///// Lấy AttributeId từ AttributeName, nếu chưa có thì tạo mới
        ///// </summary>
        //private Guid GetOrCreateAttributeId(string attributeName)
        //{
        //    if (string.IsNullOrWhiteSpace(attributeName))
        //    {
        //        return Guid.Empty;
        //    }

        //    try
        //    {
        //        // Tìm Attribute theo tên
        //        var attributes = _attributeBll.GetAll();
        //        var existingAttribute = attributes.FirstOrDefault(a => 
        //            !string.IsNullOrWhiteSpace(a.Name) && 
        //            a.Name.Trim().Equals(attributeName.Trim(), StringComparison.OrdinalIgnoreCase));

        //        if (existingAttribute != null)
        //        {
        //            return existingAttribute.Id;
        //        }

        //        // Nếu chưa có, tạo mới Attribute
        //        var newAttribute = new Attribute
        //        {
        //            Id = Guid.NewGuid(),
        //            Name = attributeName.Trim(),
        //            DataType = "String", // Mặc định là String
        //            Description = null // Có thể để null hoặc set mô tả nếu cần
        //        };

        //        _attributeBll.SaveOrUpdate(newAttribute);
        //        _logger.Info($"GetOrCreateAttributeId: Đã tạo Attribute mới với Name='{attributeName}', Id={newAttribute.Id}");
                
        //        return newAttribute.Id;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error($"GetOrCreateAttributeId: Lỗi khi lấy/tạo Attribute với Name='{attributeName}': {ex.Message}", ex);
        //        throw new InvalidOperationException($"Không thể lấy hoặc tạo thuộc tính '{attributeName}': {ex.Message}", ex);
        //    }
        //}

        ///// <summary>
        ///// Kiểm tra trùng lặp ProductId và VariantFullName
        ///// </summary>
        //private async Task CheckDuplicateAsync(Guid productId, string variantFullName)
        //{
        //    try
        //    {
        //        // Lấy tất cả ProductVariant
        //        var allVariants = await _productVariantBll.GetAllAsync();
                
        //        // Filter theo ProductId và kiểm tra trùng FullName (tên đầy đủ của biến thể)
        //        var duplicate = allVariants.FirstOrDefault(v => 
        //            v.ProductId == productId &&
        //            !string.IsNullOrWhiteSpace(v.FullName) && 
        //            v.FullName.Trim().Equals(variantFullName.Trim(), StringComparison.OrdinalIgnoreCase));

        //        if (duplicate != null)
        //        {
        //            throw new InvalidOperationException(
        //                $"Đã tồn tại biến thể với cùng sản phẩm và tên biến thể '{variantFullName}'. Vui lòng chọn tên khác.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error($"CheckDuplicateAsync: Lỗi kiểm tra trùng lặp: {ex.Message}", ex);
        //        throw;
        //    }
        //}

        //#endregion
    }
}