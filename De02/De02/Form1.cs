using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace De02
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void headerset()
        {
            dgvQLSP.Columns[0].HeaderText = "Mã SP";
            dgvQLSP.Columns[1].HeaderText = "Tên sản phẩm";
            dgvQLSP.Columns[2].HeaderText = "Ngày nhập";
            dgvQLSP.Columns[3].HeaderText = "Loại SP";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            QLSanPhamDB qLSanPhamDb = new QLSanPhamDB();
            var query = from sp in qLSanPhamDb.Sanphams
                        join loai in qLSanPhamDb.LoaiSPs
                        on sp.MaLoai equals loai.MaLoai
                        select new
                        {
                            Mã_SP = sp.MaSP,
                            Tên_sản_phẩm = sp.TenSP,
                            Ngày_nhập = sp.Ngaynhap,
                            Loại_SP = loai.TenLoai
                        };
            dgvQLSP.DataSource = query.ToList();
            headerset();
            List<LoaiSP> loaiSPs = qLSanPhamDb.LoaiSPs.ToList();
            this.cmbSP.DataSource = loaiSPs;
            this.cmbSP.DisplayMember = "TenLoai";
            this.cmbSP.ValueMember = "MaLoai";

            DateEnter.Format = DateTimePickerFormat.Custom;
            DateEnter.CustomFormat = "dd MMMM yyyy";

        }

  
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow viewRow = dgvQLSP.Rows[e.RowIndex];
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                txtID.Text = viewRow.Cells[0].Value.ToString();
                txtName.Text = viewRow.Cells[1].Value.ToString();
                DateEnter.Text =viewRow.Cells[2].Value.ToString();
                cmbSP.Text = viewRow.Cells[3].Value.ToString();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                QLSanPhamDB qLSanPhamDb = new QLSanPhamDB();
                var query = from sp in qLSanPhamDb.Sanphams
                            join loai in qLSanPhamDb.LoaiSPs
                            on sp.MaLoai equals loai.MaLoai
                            select new
                            {
                                Mã_SP = sp.MaSP,
                                Tên_sản_phẩm = sp.TenSP,
                                Ngày_nhập = sp.Ngaynhap,
                                Loại_SP = loai.TenLoai
                            };
                if (query.Any(s => s.Mã_SP == txtID.Text))
                {
                    MessageBox.Show("Sản phẩm đã tồn tại. Vui lòng nhập một mã khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var newSP = new Sanpham
                {
                    MaSP = txtID.Text,
                    TenSP = txtName.Text,
                    Ngaynhap = DateEnter.Value.Date,
                    MaLoai = cmbSP.SelectedValue.ToString()
                };
                qLSanPhamDb.Sanphams.Add(newSP);
                qLSanPhamDb.SaveChanges();
                dgvQLSP.DataSource = null;
                dgvQLSP.DataSource = query.ToList();
                headerset();
                MessageBox.Show("Thêm Sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm dữ liệu: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
}

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                QLSanPhamDB qLSanPhamDb = new QLSanPhamDB();
               List<Sanpham> sanphams =qLSanPhamDb.Sanphams.ToList();
                var query = from sp in qLSanPhamDb.Sanphams
                            join loai in qLSanPhamDb.LoaiSPs
                            on sp.MaLoai equals loai.MaLoai
                            select new
                            {
                                Mã_SP = sp.MaSP,
                                Tên_sản_phẩm = sp.TenSP,
                                Ngày_nhập = sp.Ngaynhap,
                                Loại_SP = loai.TenLoai
                            };
                var find = sanphams.FirstOrDefault(s => s.MaSP == txtID.Text);
                if (find != null)
                {
                    find.TenSP = txtName.Text;
                    find.Ngaynhap = DateEnter.Value.Date;
                    find.MaLoai = cmbSP.SelectedValue.ToString(); 
               
                qLSanPhamDb.SaveChanges();
                dgvQLSP.DataSource = null;
                dgvQLSP.DataSource = query.ToList();
                headerset();
                MessageBox.Show("Sửa Sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi Sửa dữ liệu: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                QLSanPhamDB qLSanPhamDb = new QLSanPhamDB();
                List<Sanpham> sanphams = qLSanPhamDb.Sanphams.ToList();
                var query = from sp in qLSanPhamDb.Sanphams
                            join loai in qLSanPhamDb.LoaiSPs
                            on sp.MaLoai equals loai.MaLoai
                            select new
                            {
                                Mã_SP = sp.MaSP,
                                Tên_sản_phẩm = sp.TenSP,
                                Ngày_nhập = sp.Ngaynhap,
                                Loại_SP = loai.TenLoai
                            };
                var find = sanphams.FirstOrDefault(s => s.MaSP == txtID.Text);
                if (find != null)
                {
                    DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát không?", "Xác nhận thoát",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes){}
                    else
                    {
                        return;
                    }
                    qLSanPhamDb.Sanphams.Remove(find);
                    qLSanPhamDb.SaveChanges();
                    dgvQLSP.DataSource = null;
                    dgvQLSP.DataSource = query.ToList();
                    headerset();
                    MessageBox.Show("Xóa Sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa dữ liệu: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát không?", "Xác nhận thoát",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {

        }
    }
}
