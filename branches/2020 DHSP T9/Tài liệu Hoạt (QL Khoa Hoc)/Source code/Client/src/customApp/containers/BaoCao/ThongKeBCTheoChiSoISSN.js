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
    const {filter} = this.props;
    return (
      <table style={{...tblstyle.table}}>
        <tr>
          <th style={{...tblstyle.th, fontSize: 16, border: "none", textAlign: "center"}} colSpan={16}>
            THÔNG TIN KÊ KHAI BÀI BÁO KHOA HỌC
            <br/>
            <i style={{fontWeight: 'normal'}}>Từ năm {filter.NamBatDau} đến năm {filter.NamKetThuc}</i>
          </th>
        </tr>

        <tr/>
        <tr>
          <th style={{...tblstyle.th, width: 50}} rowSpan={3}>STT</th>
          <th style={{...tblstyle.th, width: 200}} rowSpan={3}>Tên bài báo</th>
          <th style={{...tblstyle.th, width: 150}} rowSpan={3}>Được tài trợ bởi</th>
          <th style={{...tblstyle.th, width: 150}} rowSpan={3}>Tác giả</th>
          <th style={{...tblstyle.th, width: 200}} rowSpan={1} colSpan={6}>Tạp chí/Hội thảo</th>
          <th style={{...tblstyle.th, width: 100}} rowSpan={3}>Số ISSN/ ISBN</th>
          <th style={{...tblstyle.th}} rowSpan={3}>Tập</th>
          <th style={{...tblstyle.th}} rowSpan={3}>Số</th>
          <th style={{...tblstyle.th}} rowSpan={3}>Năm đăng tải</th>
          <th style={{...tblstyle.th}} rowSpan={3}>Từ trang đến trang</th>
          <th style={{...tblstyle.th, width: 100}} rowSpan={3}>Đường link bài báo</th>
          <th style={{...tblstyle.th, width: 100}} rowSpan={3}>Lĩnh vực/ngành khoa học</th>
        </tr>
        <tr>

          <th style={{...tblstyle.th, width: 100}} rowSpan={2}>Tên tạp chí/hội thảo</th>
          <th style={{...tblstyle.th, width: 100}} colSpan={3}>Quốc tế</th>
          <th style={{...tblstyle.th, width: 100}}>Trong nước</th>
          <th style={{...tblstyle.th, width: 100}}>Hội thảo</th>

        </tr>
        <tr>
          <th style={{...tblstyle.th, width: 100}}>Hệ số ảnh hưởng</th>
          <th style={{...tblstyle.th, width: 100}}>ISI/SCOPUS/Khác</th>
          <th style={{...tblstyle.th, width: 100}}>Rank theo SCIMAGO</th>
          <th style={{...tblstyle.th, width: 100}}>Điểm tạp chí</th>
          <th style={{...tblstyle.th, width: 100}}>Loại Hội thảo</th>

        </tr>
        {data.map((item, index) => (
          <tr style={{...tblstyle.tr}}>
            <td style={{...tblstyle.td, textAlign: "center"}}>{index + 1}</td>
            <td style={{...tblstyle.td, textAlign: "left"}}>{item.TenBaiBao}</td>
            <td style={{...tblstyle.td, textAlign: "center"}}>{item.MaDeTai}</td>

            <td style={{...tblstyle.td, textAlign: "left"}}>{item.TenCacTacGia}</td>
            <td style={{...tblstyle.td, textAlign: "left"}}>{item.TenTapChiHoiThao}</td>
            <td style={{...tblstyle.td, textAlign: "left"}}>{item.HeSoAnhHuong}</td>
            <td style={{...tblstyle.td, textAlign: "center"}}>{item.ISI_SCOPUS}</td>
            <td style={{...tblstyle.td, textAlign: "center"}}>{item.Rank_SCIMAGO}</td>
            <td style={{...tblstyle.td, textAlign: "center"}}>{item.DiemTapChi}</td>
            <td style={{...tblstyle.td, textAlign: "center"}}>{item.LoaiHoiThao}</td>
            <td style={{...tblstyle.td, textAlign: "center"}}>{item.ISSN}</td>
            <td style={{...tblstyle.td, textAlign: "left"}}>{item.Tap}</td>
            <td style={{...tblstyle.td, textAlign: "left"}}>{item.So}</td>
            <td style={{...tblstyle.td, textAlign: "center"}}>{item.NamDangTai}</td>
            <td style={{...tblstyle.td, textAlign: "left"}}>{item.TuTrangDenTrang}</td>
            <td style={{...tblstyle.td, textAlign: "left"}}><a href={item.LinkBaiBao}
                                                               target={'_blank'}>{item.LinkBaiBao}</a></td>
            <td style={{...tblstyle.td, textAlign: "left"}}>{item.LinhVucNganhKhoaHoc}</td>

            {/* <td style={{ ...tblstyle.td, textAlign: "center" }}>
              {item.TongKinhPhi != 0 ? numeral(item.TongKinhPhi).format('0,0') : ""}
            </td>
       */}
            {/* <td style={{ ...tblstyle.td, textAlign: "left" }}>{item.GhiChu}</td> */}
          </tr>
        ))}
      </table>
    );
  }
}
