import React, { Component } from "react";
import numeral from "numeral";
const tblstyle = {
  th: {
    textAlign: "center",
    fontWeight: "bold",
    border: "solid 0.5pt #999",
    verticalAlign: "middle",
    padding: 5,
  },
  td: {
    border: "solid 0.5pt #999",
    padding: 5,
    verticalAlign: "middle",
  },
};
export default class QLDeXuat extends Component {
  constructor(props) {
    super(props);
    this.formPrint = React.createRef();
  }
  componentDidMount() {
    setTimeout(() => {
      this.exportExcel();

      this.props.callBack();
    }, 1000);
  }
  exportExcel = () => {
    let html, link, blob, url;
    let preHtml = `<html><head><meta charset='utf-8'></head><body>`;
    let postHtml = "</body></html>";
    html = preHtml + this.formPrint.current.innerHTML + postHtml;
    blob = new Blob(["\ufeff", html], {
      type: "application/vnd.ms-excel",
    });
    url = URL.createObjectURL(blob);
    link = document.createElement("A");
    link.href = url;
    link.download = "DS_DeTai.xls"; // default name without extension
    document.body.appendChild(link);
    if (navigator.msSaveOrOpenBlob) navigator.msSaveOrOpenBlob(blob, "DS_DeTai.xls");
    // IE10-11
    else link.click(); // other browsers
    document.body.removeChild(link);
  };

  render() {
    const { data } = this.props;
    return (
      <div className="d-none">
        <div ref={this.formPrint} id={"form-print"} style={{ display: "none" }}>
          <table style={{ fontFamily: "Times New Roman" }}>
            <thead>
              <tr>
                <th style={{ ...tblstyle.th, fontSize: 20, border: "none" }} colSpan={20}>
                  DANH SÁCH ĐỀ TÀI
                </th>
              </tr>
              <tr></tr>
              <tr>
                <th style={{ textAlign: "center", fontWeight: "bold", border: "solid 0.5pt", verticalAlign: "middle", padding: 5 }}>STT</th>
                <th style={{ textAlign: "center", fontWeight: "bold", border: "solid 0.5pt", verticalAlign: "middle", padding: 5 }}>Mã nhiệm vụ</th>
                <th style={{ textAlign: "center", fontWeight: "bold", border: "solid 0.5pt", verticalAlign: "middle", padding: 5 }}>Tên nhiệm vụ</th>
                <th style={{ textAlign: "center", fontWeight: "bold", border: "solid 0.5pt", verticalAlign: "middle", padding: 5 }}>Chủ nhiệm đề tài</th>
                <th style={{ textAlign: "center", fontWeight: "bold", border: "solid 0.5pt", verticalAlign: "middle", padding: 5 }}>Loại hình nghiên cứu</th>
                <th style={{ textAlign: "center", fontWeight: "bold", border: "solid 0.5pt", verticalAlign: "middle", padding: 5 }}>Lĩnh vực nghiên cứu</th>
                <th style={{ textAlign: "center", fontWeight: "bold", border: "solid 0.5pt", verticalAlign: "middle", padding: 5 }}>Lĩnh vực KT-XH</th>
                <th style={{ textAlign: "center", fontWeight: "bold", border: "solid 0.5pt", verticalAlign: "middle", padding: 5 }}>Cấp quản lý</th>
                <th style={{ textAlign: "center", fontWeight: "bold", border: "solid 0.5pt", verticalAlign: "middle", padding: 5 }}>Cơ quan chủ quản</th>
                <th style={{ textAlign: "center", fontWeight: "bold", border: "solid 0.5pt", verticalAlign: "middle", padding: 5 }}>Đơn vị quản lý khoa học</th>
                <th style={{ textAlign: "center", fontWeight: "bold", border: "solid 0.5pt", verticalAlign: "middle", padding: 5 }}>Năm bắt đầu</th>
                <th style={{ textAlign: "center", fontWeight: "bold", border: "solid 0.5pt", verticalAlign: "middle", padding: 5 }}>Năm kết thúc</th>
                <th style={{ textAlign: "center", fontWeight: "bold", border: "solid 0.5pt", verticalAlign: "middle", padding: 5 }}>Trạng thái</th>
                <th style={{ textAlign: "center", fontWeight: "bold", border: "solid 0.5pt", verticalAlign: "middle", padding: 5 }}>Kinh phí </th>

                <th style={{ textAlign: "center", fontWeight: "bold", border: "solid 0.5pt", verticalAlign: "middle", padding: 5 }}>Thành viên tham gia</th>
                <th style={{ textAlign: "center", fontWeight: "bold", border: "solid 0.5pt", verticalAlign: "middle", padding: 5 }}>Đơn vị phối hợp</th>
                <th style={{ textAlign: "center", fontWeight: "bold", border: "solid 0.5pt", verticalAlign: "middle", padding: 5 }}>Mục tiêu</th>
                <th style={{ textAlign: "center", fontWeight: "bold", border: "solid 0.5pt", verticalAlign: "middle", padding: 5 }}>Các nội dung chính</th>
                <th style={{ textAlign: "center", fontWeight: "bold", border: "solid 0.5pt", verticalAlign: "middle", padding: 5 }}>Sản phẩm đăng ký</th>
                <th style={{ textAlign: "center", fontWeight: "bold", border: "solid 0.5pt", verticalAlign: "middle", padding: 5 }}>Khả năng ứng dụng của đề tài</th>
              </tr>
            </thead>
            <tbody>
              {data.map((item, index) => (
                <tr>
                  <td style={{ textAlign: "center", border: "solid 0.5pt", padding: 5, verticalAlign: "middle" }}>{index + 1}</td>
                  <td style={{ textAlign: "center", border: "solid 0.5pt", padding: 5, verticalAlign: "middle" }}>{item.MaDeTai}</td>
                  <td style={{ border: "solid 0.5pt", padding: 5, verticalAlign: "middle" }}>{item.TenDeTai}</td>
                  <td style={{ border: "solid 0.5pt", padding: 5, verticalAlign: "middle" }}>{item.TenChuNhiemDeTai}</td>
                  <td style={{ border: "solid 0.5pt", padding: 5, verticalAlign: "middle" }}>{item.TenLoaiHinhNghienCuu}</td>
                  <td style={{ border: "solid 0.5pt", padding: 5, verticalAlign: "middle" }}>{item.TenLinhVucNghienCuu}</td>
                  <td style={{ border: "solid 0.5pt", padding: 5, verticalAlign: "middle" }}>{item.TenLinhVucKinhTeXaHoi}</td>
                  <td style={{ border: "solid 0.5pt", padding: 5, verticalAlign: "middle" }}>{item.TenCapQuanLy}</td>
                  <td style={{ border: "solid 0.5pt", padding: 5, verticalAlign: "middle" }}>{item.CoQuanChuQuan}</td>
                  <td style={{ border: "solid 0.5pt", padding: 5, verticalAlign: "middle" }}>{item.DonViQuanLyKhoaHoc}</td>
                  <td style={{ textAlign: "center", border: "solid 0.5pt", padding: 5, verticalAlign: "middle" }}>{item.NamBatDau}</td>
                  <td style={{ textAlign: "center", border: "solid 0.5pt", padding: 5, verticalAlign: "middle" }}>{item.NamKetThuc}</td>
                  <td style={{ textAlign: "center", border: "solid 0.5pt", padding: 5, verticalAlign: "middle" }}>
                    {item.TrangThai === 1 ? "Đang thực hiện" : item.TrangThai === 2 ? "Nghiệm thu" : "Thanh lý"}
                  </td>
                  <td style={{ textAlign: "center", border: "solid 0.5pt", padding: 5, verticalAlign: "middle" }}>{item.KinhPhiDHSP === 0 ? "" : numeral(item.KinhPhiDHSP).format("0,0")}</td>

                  <td style={{ border: "solid 0.5pt", padding: 5, verticalAlign: "middle" }}>{item.ThanhVienNghienCuuStr}</td>
                  <td style={{ border: "solid 0.5pt", padding: 5, verticalAlign: "middle" }}>{item.DonViPhoiHop}</td>
                  <td style={{ border: "solid 0.5pt", padding: 5, verticalAlign: "middle" }} dangerouslySetInnerHTML={{ __html: item.MucTieu }}></td>
                  <td style={{ border: "solid 0.5pt", padding: 5, verticalAlign: "middle" }}>{item.CacNoiDungChinhStr}</td>
                  <td style={{ border: "solid 0.5pt", padding: 5, verticalAlign: "middle" }} dangerouslySetInnerHTML={{ __html: item.SanPhamDangKy }}></td>
                  <td style={{ border: "solid 0.5pt", padding: 5, verticalAlign: "middle" }} dangerouslySetInnerHTML={{ __html: item.KhaNangUngDung }}></td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    );
  }
}
