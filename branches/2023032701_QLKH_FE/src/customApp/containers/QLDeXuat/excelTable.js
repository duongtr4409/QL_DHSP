import React, { Component } from "react";
import numeral from "numeral";
import moment from "moment";

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
      if (this.props.command === "export") {
        this.exportExcel();
      }
      if (this.props.command === "print") {
        this.printPDF();
      }

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
    link.download = "DS_DeXuat.xls"; // default name without extension
    document.body.appendChild(link);
    if (navigator.msSaveOrOpenBlob) navigator.msSaveOrOpenBlob(blob, "DS_DeXuat.xls");
    // IE10-11
    else link.click(); // other browsers
    document.body.removeChild(link);
  };

  printPDF = () => {
    //xoa iframe cu ------
    let oldIframe = document.querySelectorAll("iframe");
    if (oldIframe && oldIframe.length) {
      oldIframe.forEach((element) => {
        element.parentNode.removeChild(element);
      });
    }
    //tao iframe moi -----
    let node = this.formPrint.current.innerHTML;
    let iframe = document.createElement("iframe");
    iframe.style.display = "none";
    document.body.appendChild(iframe); //make document #html in iframe
    iframe.contentWindow.document.open();
    iframe.contentWindow.document.write(node);
    iframe.contentWindow.document.close();
    iframe.contentWindow.focus();
    iframe.contentWindow.print();
  };

  render() {
    const { data } = this.props;

    return (
      <div className="d-none">
        <div
          ref={this.formPrint}
          id={"form-print"}
          // style={{ display: "none" }}
        >
          <div style={{ fontSize: 20, border: "none", textAlign: "center" }}>DANH SÁCH ĐỀ XUẤT</div>
          <br></br>
          <table style={{ fontFamily: "Times New Roman", borderCollapse: "collapse" }}>
            <thead>
              <tr>
                <th style={{ ...tblstyle.th, width: 50 }}>STT</th>
                <th style={{ ...tblstyle.th, width: 150 }}> Ngày đề xuất</th>
                <th style={{ ...tblstyle.th, width: 150 }}> Mã đề xuất</th>
                <th style={{ ...tblstyle.th, width: 200 }}>Tên đề xuất</th>
                <th style={{ ...tblstyle.th, width: 150 }}>Lĩnh vực nghiên cứu</th>
                <th style={{ ...tblstyle.th, width: 150 }}>Lĩnh vực KT-XH</th>
                <th style={{ ...tblstyle.th, width: 150 }}>Cấp quản lý</th>
                <th style={{ ...tblstyle.th, width: 150 }}>Người đề xuất</th>

                <th style={{ ...tblstyle.th, width: 150 }}>Tính cấp thiết</th>
                <th style={{ ...tblstyle.th, width: 150 }}>Mục tiêu</th>
                <th style={{ ...tblstyle.th, width: 150 }}>Nội dung</th>
                <th style={{ ...tblstyle.th, width: 150 }}>Sản phẩm</th>

                <th style={{ ...tblstyle.th, width: 150 }}>Kinh phí dự kiến</th>

                <th style={{ ...tblstyle.th, width: 150 }}>Thời gian nghiên cứu</th>

                <th style={{ ...tblstyle.th, width: 150 }}>Thời gian thực hiện</th>

                <th style={{ ...tblstyle.th, width: 150 }}>Thuộc chương trình</th>
                <th style={{ ...tblstyle.th, width: 150 }}>Địa chỉ ứng dụng</th>
              </tr>
            </thead>
            <tbody>
              {data.map((item, index) => (
                <tr>
                  <td style={{ ...tblstyle.td, textAlign: "center" }}>{index + 1}</td>
                  <td style={{ ...tblstyle.td }}>{item.NgayThucHien ? moment(item.NgayThucHien).format("DD/MM/YYYY") : ""}</td>
                  <td style={{ ...tblstyle.td }}>{item.MaDeXuat}</td>
                  <td style={{ ...tblstyle.td }}>{item.TenDeXuat}</td>
                  <td style={{ ...tblstyle.td }}>{item.TenLinhVucNghienCuu}</td>
                  <td style={{ ...tblstyle.td }}>{item.TenLinhVucKinhTeXaHoi}</td>
                  <td style={{ ...tblstyle.td }}>{item.TenCapQuanLy}</td>
                  <td style={{ ...tblstyle.td }}>{item.TenNguoiDeXuat}</td>

                  <td style={{ ...tblstyle.td }} dangerouslySetInnerHTML={{ __html: item.TinhCapThiet }}></td>
                  <td style={{ ...tblstyle.td }} dangerouslySetInnerHTML={{ __html: item.MucTieu }}></td>
                  <td style={{ ...tblstyle.td }}>{`${item.NoiDung}`}</td>
                  <td style={{ ...tblstyle.td }} dangerouslySetInnerHTML={{ __html: item.SanPham }}></td>

                  <td style={{ ...tblstyle.td, textAlign: "left" }}>{item.KinhPhiDuKien === 0 ? "" : numeral(item.KinhPhiDuKien).format("0,0")}</td>

                  <td style={{ ...tblstyle.td, textAlign: "left" }}>{item.ThoiGianNghienCuu}</td>

                  <td style={{ ...tblstyle.td }}>
                    {item.ThoiGianThucHienTu ? item.ThoiGianThucHienTu : ""}
                    {`${item.ThoiGianThucHienDen ? ` đến ${item.ThoiGianThucHienDen}` : ""}`}
                  </td>

                  <td style={{ ...tblstyle.td }}>{`${item.ThuocChuongTrinh}`}</td>
                  <td style={{ ...tblstyle.td }}>{item.DiaChiUngDung}</td>

                  {/* <td style={{ ...tblstyle.td }} dangerouslySetInnerHTML={{ __html: item.MucTieu }}></td> */}
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    );
  }
}
