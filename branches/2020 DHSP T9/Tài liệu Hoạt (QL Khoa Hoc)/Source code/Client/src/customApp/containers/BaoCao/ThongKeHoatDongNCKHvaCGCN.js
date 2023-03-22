import React from "react";
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

  renderContentKhocHoc = (data) => {
    const dataArr = data.split(";");
    return <ul>
      {dataArr.map(item => (
        <li>{item.split(",").join(", ")}</li>
      ))}
    </ul>
  };

  render() {
    const data = this.props.data || [];
    return (
      <table style={tblstyle.table}>
        <colgroup span="4"></colgroup>
        <tr style={tblstyle.tr}>
          <th style={{...tblstyle.th, fontSize: 16, border: "none"}} colSpan={12}>
            <p style={{marginTop: 16, marginBottom: 16}}>CÁC HOẠT ĐỘNG NGHIÊN CỨU KHOA HỌC, CHUYỂN GIAO CÔNG NGHỆ, SẢN
              XUẤT THƯ VÀ TƯ VẤN</p>
          </th>
        </tr>

        <tr/>
        <tr style={tblstyle.tr}>
          <th rowSpan="2" style={{...tblstyle.th, width: 50}}>
            STT
          </th>
          <th rowSpan="2" style={{...tblstyle.th, width: 100}}>
            Mã đề tài
          </th>
          <th rowSpan="2" style={{...tblstyle.th, width: 250}}>
            Tên dự án, nhiệm vụ, khoa học công nghệ
          </th>
          <th rowSpan="2" style={{...tblstyle.th, width: 100}}>
            Người chủ trì và các thành viên
          </th>
          <th rowSpan="2" style={{...tblstyle.th, width: 100}}>
            Đối tác trong nước và quốc tế{" "}
          </th>
          <th rowSpan="2" style={{...tblstyle.th, width: 100}}>
            Thời gian thực hiện
          </th>
          <th rowSpan="2" style={{...tblstyle.th, width: 100}}>
            Đang thực hiện/ Nghiệm thu/ Thanh lý
          </th>
          <th rowSpan="2" style={{...tblstyle.th, width: 100}}>
            Kinh phí thực hiện (triệu đồng)
          </th>
          <th colSpan="4" scope="colgroup" style={{...tblstyle.th, width: 400}}>
            Sản phẩm ứng dụng thực tiễn đã hoàn thành của đề tài
          </th>
        </tr>
        <tr style={tblstyle.tr}>
          <th scope="col" style={{...tblstyle.th, width: 100}}>
            Bài báo ISI, Scopus, bài trong nước, bài hội thảo quốc tế, hội thảo trong nước
          </th>
          <th scope="col" style={{...tblstyle.th, width: 100}}>
            Sách chuyên khảo, giáo trình, sách tham khảo, tài liệu tham khảo
          </th>
          <th scope="col" style={{...tblstyle.th, width: 100}}>
            Đào tạo Cử nhân, thạc sĩ, hỗ trợ đào tạo NCS
          </th>
          <th scope="col" style={{...tblstyle.th, width: 100}}>
            Sản phẩm khác, sản phẩm ứng dụng thực tiễn
          </th>
        </tr>
        {data.map((item, index) => (
          <tr style={tblstyle.tr}>
            <td style={{...tblstyle.td, textAlign: "center"}}>{index + 1}</td>
            <td style={{...tblstyle.td, textAlign: "left"}}>{item.MaDeTai}</td>
            <td style={{...tblstyle.td, textAlign: "left"}}>{item.TenNhiemVu}</td>
            <td style={{...tblstyle.td, textAlign: "left"}}>{item.NguoiChuTri}</td>
            <td style={{...tblstyle.td, textAlign: "left"}}>{item.DoiTac}</td>
            <td style={{...tblstyle.td, textAlign: "left"}}>{item.ThoiGianThucHien}</td>
            <td style={{...tblstyle.td, textAlign: "left"}}>{item.TrangThaiThucHien}</td>
            <td
              style={{...tblstyle.td, textAlign: "center"}}>{item.KinhPhiThucHien != 0 ? item.KinhPhiThucHien : ""}</td>
            <td style={{...tblstyle.td, textAlign: "left"}}>{this.renderContentKhocHoc(item.BaiBao)}</td>
            <td style={{...tblstyle.td, textAlign: "left"}}>{this.renderContentKhocHoc(item.SachChuyenKhao)}</td>
            <td style={{...tblstyle.td, textAlign: "left"}}>{this.renderContentKhocHoc(item.DaoTao)}</td>
            <td style={{...tblstyle.td, textAlign: "left"}}>{this.renderContentKhocHoc(item.SanPhamKhac)}</td>
          </tr>
        ))}
      </table>
    );
  }
}
