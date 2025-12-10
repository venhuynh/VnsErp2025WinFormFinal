using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bll.MasterData.CompanyBll;
using Common.Common;
using DTO.MasterData.Company;

namespace MasterData.Company
{
    public partial class FrmEmployeeDtoDetail : DevExpress.XtraEditors.XtraForm
    {
        private readonly EmployeeBll _employeeBll = new();
        private readonly Guid? _employeeId;

        /// <summary>
        /// Constructor cho thêm mới nhân viên
        /// </summary>
        public FrmEmployeeDtoDetail()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor cho điều chỉnh nhân viên
        /// </summary>
        /// <param name="employeeId">ID của nhân viên cần điều chỉnh</param>
        public FrmEmployeeDtoDetail(Guid employeeId)
        {
            InitializeComponent();
            _employeeId = employeeId;
            LoadEmployeeData();
        }

        /// <summary>
        /// Load dữ liệu nhân viên vào form (nếu là chế độ edit)
        /// </summary>
        private void LoadEmployeeData()
        {
            try
            {
                if (_employeeId == null || _employeeId == Guid.Empty) return;

                var employee = _employeeBll.GetById(_employeeId.Value);
                if (employee == null)
                {
                    MsgBox.ShowWarning("Không tìm thấy nhân viên cần điều chỉnh.");
                    return;
                }

                // TODO: Bind dữ liệu employee vào các controls trong form
                // Ví dụ:
                // txtEmployeeCode.Text = employee.EmployeeCode;
                // txtFullName.Text = employee.FullName;
                // ...
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(new Exception("Lỗi load dữ liệu nhân viên: " + ex.Message, ex));
            }
        }
    }
}