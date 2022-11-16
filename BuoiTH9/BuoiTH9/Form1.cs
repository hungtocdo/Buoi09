using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace BuoiTH9
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        DataSet ds = new DataSet("dsQLNV");
        SqlDataAdapter daChucVu;
        SqlDataAdapter daNhanVien;
        
       
        private void Form1_Load_1(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source=DESKTOP-796RQ1F\HUNG;Initial Catalog=QLNV;Integrated Security=True";
            //D lieu combobox Chuc vu
            string sQueryChucVu = @"select * from chucvu";
            daChucVu = new SqlDataAdapter(sQueryChucVu, conn);
            daChucVu.Fill(ds, "tblChucVu");
            cboChucVu.DataSource = ds.Tables["tblChucVu"];
            cboChucVu.DisplayMember = "tencv";
            cboChucVu.ValueMember = "macv";
            

            //Du lieu datagrid Danh sach nhan vien

            string sQueryNhanVien = @"select n.*, c.tencv from nhanvien n, chucvu c where n.macv = c.macv";
            daNhanVien = new SqlDataAdapter(sQueryNhanVien, conn);
            daNhanVien.Fill(ds, "tblDSNhanVien");
            dgDSNhanVien.DataSource = ds.Tables["tblDSNhanVien"];

            dgDSNhanVien.Columns["manv"].HeaderText = "Mã Số";
            dgDSNhanVien.Columns["manv"].Width = 100;

            

            dgDSNhanVien.Columns["holot"].HeaderText = "Họ lót";
            dgDSNhanVien.Columns["holot"].Width = 170;

            dgDSNhanVien.Columns["tennv"].HeaderText = "Tên";
            dgDSNhanVien.Columns["tennv"].Width = 100;

            dgDSNhanVien.Columns["ngaysinh"].HeaderText = "Ngày Sinh";
            dgDSNhanVien.Columns["ngaysinh"].Width = 170;
          

            dgDSNhanVien.Columns["phai"].HeaderText = "Phái";
            dgDSNhanVien.Columns["phai"].Width = 90;

            dgDSNhanVien.Columns["macv"].HeaderText = "Mã CV";
            dgDSNhanVien.Columns["macv"].Width = 120;

            dgDSNhanVien.Columns["tencv"].HeaderText = "Tên Chức vụ";
            dgDSNhanVien.Columns["tencv"].Width = 170;

            


            //command Them nhan vien
            string sThemNV = @"insert into nhanvien values(@MaNV, @HoLot ,@TenNV, @Phai, @NgaySinh, @MaCV)";
            SqlCommand cmThemNV = new SqlCommand(sThemNV, conn);
            cmThemNV.Parameters.Add("@MaNV", SqlDbType.NVarChar, 5, "manv");
            cmThemNV.Parameters.Add("@HoLot", SqlDbType.NVarChar, 50, "holot");
            cmThemNV.Parameters.Add("@TenNV", SqlDbType.NVarChar, 10, "tennv");
            cmThemNV.Parameters.Add("@NgaySinh", SqlDbType.SmallDateTime, 10, "ngaysinh");
            cmThemNV.Parameters.Add("@MaCv", SqlDbType.NVarChar, 5, "mavc");
            daNhanVien.InsertCommand = cmThemNV;


            //luu nhan vien
            string sQueryUpdate = @"update nhanvien set holot = @HoLot, tennv= @TenNV, phai = @Phai, ngaysinh= @NgaySinh, macv= @MaCV  WHERE manv = @MaNV";
            SqlCommand comd = new SqlCommand(sQueryUpdate, conn);
            comd.Parameters.Add("@HoLot", SqlDbType.NVarChar, 50, "holot");
            comd.Parameters.Add("@TenNV", SqlDbType.NVarChar, 10, "tennv");
            comd.Parameters.Add("@Phai", SqlDbType.NVarChar, 3, "phai");
            comd.Parameters.Add("@NgaySinh", SqlDbType.SmallDateTime, 10, "ngaysinh");
            comd.Parameters.Add("@MaCV", SqlDbType.NVarChar, 5, "macv");
            SqlParameter parmUpdate = comd.Parameters.Add("@MaNV", SqlDbType.NVarChar, 5, "manv");
            parmUpdate.SourceVersion = DataRowVersion.Original;
            daNhanVien.UpdateCommand = comd;
        }

        private void dgDSNhanVien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow dr = dgDSNhanVien.SelectedRows[0];
            txtMaSo.Text = dr.Cells["manv"].Value.ToString();
            txtHo.Text = dr.Cells["holot"].Value.ToString();
            txtTen.Text = dr.Cells["tennv"].Value.ToString();
            if (dr.Cells["phai"].Value.ToString() == "Nam")
            {
                radbNAm.Checked = true;
            }
            else
            {
                radbNu.Checked = true;
            }
            dtpNgaySInh.Text = dr.Cells["ngaysinh"].Value.ToString();
            cboChucVu.SelectedValue = dr.Cells["macv"].Value.ToString();
              
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            DataRow row = ds.Tables["tblDSNhanVien"].NewRow();
            row["manv"] = txtMaSo.Text;
            row["holot"] = txtHo.Text;
            row["tennv"] = txtTen.Text;
           
            if(radbNu.Checked == true)
            {
                row["phai"] = "Nữ";
            }
            else
            {
                row["phai"] = "Nam";
            }
            row["ngaysinh"] = dtpNgaySInh.Text;
            row["macv"] = cboChucVu.Text;
            row["tencv"] = cboChucVu.Text;
            ds.Tables["tblDSNhanVien"].Rows.Add(row);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            foreach(DataGridViewRow item in this.dgDSNhanVien.SelectedRows)
            {
                dgDSNhanVien.Rows.RemoveAt(item.Index);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            DataGridViewRow r = dgDSNhanVien.SelectedRows[0];
            r.Cells["manv"].Value = txtMaSo.Text;
            r.Cells["holot"].Value = txtHo.Text;
            r.Cells["tennv"].Value = txtTen.Text;

            if (radbNu.Checked == true)
            {
                r.Cells["phai"].Value = "Nữ";
            }
            else
            {
                r.Cells["phai"].Value = "Nam";
            }
            r.Cells["ngaysinh"].Value = dtpNgaySInh.Text;
            r.Cells["macv"].Value = cboChucVu.Text;
            r.Cells["tencv"].Value = cboChucVu.Text;
            

        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            
            dtpNgaySInh.ResetText();
            cboChucVu.ResetText();
            
            txtHo.ResetText();
            txtMaSo.ResetText();
            txtTen.ResetText();

        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
           
                this.BindingContext[ds, "nhanvien"].EndCurrentEdit();
                if (ds.HasChanges() == true)
                {
                    if (MessageBox.Show("Bạn có thực sự muốn lưu các thay đổi trên bảng dữ liệu Danh sách nhân viên ?", "Xác nhận thay đổi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                    try
                    {
                        daNhanVien.Update(ds, "nhanvien");
                        MessageBox.Show("Đã cập nhật");
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show(x.Message);
                    }
                    }
                }
                else
                {
                    MessageBox.Show("Cơ sở dữ liệu chưa có sự thay đổi.", "Thông báo người dùng", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            
        }
    }
}
