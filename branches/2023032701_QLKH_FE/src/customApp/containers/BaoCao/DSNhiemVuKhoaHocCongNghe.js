import React from "react";
import moment from "moment";
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
  table: {
    pageBreakBefore: 'always',
    width: "100%",
    borderCollapse: 'collapse',
    fontFamily: "Times New Roman"
  },
  tr: {
    pageBreakInside: 'avoid',
    pageBreakAfter: 'always',
  }
};
export default class Report extends React.Component {
  constructor(props) {
    super(props);
    this.formPrint = React.createRef();
  }

  removeHtml = (html) => {
    const htmlReg = /(<([^>]+)>)/ig;
    return html.replace(htmlReg, " ");
  };

  render() {
    const currYear = moment().format('YYYY');
    const data = this.props.data || [];
    return (
      <table style={{...tblstyle.table}}>
        <tr>
          <th style={{...tblstyle.th, fontSize: 16, border: "none", textAlign: "center"}} colSpan={16}>
            DANH SÁCH CÁC NHIỆM VỤ KHOA HỌC VÀ CÔNG NGHỆ
          </th>
        </tr>
        <tr>
          <th style={{...tblstyle.th, fontSize: 14, border: "none", textAlign: "center"}} colSpan={16}>
            Năm {currYear}
          </th>
        </tr>
        <tr/>
        <tr>
          <th style={{...tblstyle.th, width: 50}} rowSpan={2}>STT</th>
          <th style={{...tblstyle.th, width: 200}} rowSpan={2}>Tên nhiệm vụ</th>
          <th style={{...tblstyle.th, width: 150}} rowSpan={2}>Chủ nhiệm đề tài</th>
          <th style={{...tblstyle.th, width: 150}} rowSpan={2}>Mục tiêu</th>
          <th style={{...tblstyle.th, width: 200}} rowSpan={2}>Kết quả sản phẩm</th>
          <th style={{...tblstyle.th, width: 100}} rowSpan={2}>Thời gian thực hiện</th>
          <th style={{...tblstyle.th}} colSpan={3}>Tổng kinh phí</th>
          <th style={{...tblstyle.th}} colSpan={3}>Kinh phí đã được cấp</th>
          <th style={{...tblstyle.th}} colSpan={3}>Kinh phí năm {currYear}</th>
          <th style={{...tblstyle.th, width: 100}} rowSpan={2}>Ghi chú</th>
        </tr>
        <tr>
          <th style={{...tblstyle.th, width: 100}}>Tổng</th>
          <th style={{...tblstyle.th, width: 100}}>NSNN</th>
          <th style={{...tblstyle.th, width: 100}}>Nguồn khác</th>
          <th style={{...tblstyle.th, width: 100}}>Tổng</th>
          <th style={{...tblstyle.th, width: 100}}>NSNN</th>
          <th style={{...tblstyle.th, width: 100}}>Nguồn khác</th>
          <th style={{...tblstyle.th, width: 100}}>Tổng</th>
          <th style={{...tblstyle.th, width: 100}}>NSNN</th>
          <th style={{...tblstyle.th, width: 100}}>Nguồn khác</th>
        </tr>
        {data.map((item, index) => (
          <tr style={{...tblstyle.tr}}>
            <td style={{...tblstyle.td, textAlign: "center"}}>{index + 1}</td>
            <td style={{...tblstyle.td, textAlign: "left"}}>{item.TenNhiemVu}</td>
            <td style={{...tblstyle.td, textAlign: "left"}}>{item.TenChuNhiemDeTai}</td>
            <td style={{...tblstyle.td, textAlign: "left"}}>{this.removeHtml(item.MucTieu)}</td>
            <td style={{...tblstyle.td, textAlign: "left"}}>{this.removeHtml(item.KetQuaSanPham)}</td>
            <td style={{...tblstyle.td, textAlign: "center"}}>
              {item.ThoiGianThucHien}</td>
            <td style={{...tblstyle.td, textAlign: "center"}}>
              {item.TongKinhPhi != 0 ? numeral(item.TongKinhPhi).format('0,0') : ""}
            </td>
            <td style={{...tblstyle.td, textAlign: "center"}}>
              {item.NSNN != 0 ? numeral(item.NSNN).format('0,0') : ""}
            </td>
            <td style={{...tblstyle.td, textAlign: "center"}}>
              {item.NguonKhac != 0 ? numeral(item.NguonKhac).format('0,0') : ""}
            </td>
            <td style={{...tblstyle.td, textAlign: "center"}}>
              {item.TongDaCap != 0 ? numeral(item.TongDaCap).format('0,0') : ""}
            </td>
            <td style={{...tblstyle.td, textAlign: "center"}}>
              {item.NSNNDaCap != 0 ? numeral(item.NSNNDaCap).format('0,0') : ""}
            </td>
            <td style={{...tblstyle.td, textAlign: "center"}}>
              {item.NguonKhacDaCap != 0 ? numeral(item.NguonKhacDaCap).format('0,0') : ""}
            </td>
            <td style={{...tblstyle.td, textAlign: "center"}}>
              {item.Tong_NamHienTai != 0 ? numeral(item.Tong_NamHienTai).format('0,0') : ""}
            </td>
            <td style={{...tblstyle.td, textAlign: "center"}}>
              {item.NSNN_NamHienTai != 0 ? numeral(item.NSNN_NamHienTai).format('0,0') : ""}
            </td>
            <td style={{...tblstyle.td, textAlign: "center"}}>
              {item.NguonKhac_NamHienTai != 0 ? numeral(item.NguonKhac_NamHienTai).format('0,0') : ""}
            </td>
            <td style={{...tblstyle.td, textAlign: "left"}}>{item.GhiChu}</td>
          </tr>
        ))}
      </table>
    );
  }
}
