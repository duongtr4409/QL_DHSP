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
    pageBreakBefore: "always",
    width: "100%",
    borderCollapse: "collapse",
    fontFamily: "Times New Roman"
  },
  tr: {
    pageBreakInside: "avoid",
    pageBreakAfter: "always",
  },
};
export default class Report extends React.Component {
  constructor(props) {
    super(props);
    this.formPrint = React.createRef();
  }

  render() {
    const data = this.props.data || [];
    const {filter} = this.props;
    // console.log(filter);
    const NamBatDau = filter && filter.NamBatDau ? filter.NamBatDau : "2020";
    const NamKetThuc = filter && filter.NamKetThuc ? filter.NamKetThuc : "2020";
    return (
      <table style={{...tblstyle.table}}>
        <tr>
          <th style={{...tblstyle.th, fontSize: 16, border: "none", textAlign: "center"}} colSpan={6}>
            NHIỆM VỤ KHOA HỌC VÀ CÔNG NGHỆ
          </th>
          <td colSpan={2}>- Đơn vị báo cáo</td>
        </tr>
        <tr>
          <th style={{...tblstyle.th, fontSize: 14, border: "none", textAlign: "center"}} colSpan={6}>
            Từ ngày {`01/01/${NamBatDau}`} Đến ngày {`31/12/${NamKetThuc}`}
          </th>
          <td colSpan={2}>- Đơn vị nhận báo cáo</td>
        </tr>
        <tr>
          <td style={{...tblstyle.th, fontSize: 14, border: "none", textAlign: "center"}} colSpan={6}/>
          <td colSpan={2}>
            <i>Đơn vị tính: Nhiệm vụ</i>
          </td>
        </tr>
        <tr/>
        <tr/>
        <tr>
          <th style={{...tblstyle.th, width: 400}} rowSpan={3}/>
          <th style={{...tblstyle.th, width: 100}} rowSpan={3}>
            Mã số
          </th>
          <th style={{...tblstyle.th, width: 100}} rowSpan={3}>
            Tổng số
          </th>
          <th style={{...tblstyle.th, width: 100}} rowSpan={3}>
            Trong đó: Số nhiệm vụ KH & CN chủ nhiệm là nữ
          </th>
          <th style={{...tblstyle.th}} colSpan={4}>
            Tình trạng tiến hành
          </th>
        </tr>
        <tr>
          <th style={{...tblstyle.th}} colSpan={2}>
            Số đang tiến hành
          </th>
          <th style={{...tblstyle.th, width: 100}} rowSpan={2}>
            Số được nghiệm thu
          </th>
          <th style={{...tblstyle.th, width: 100}} rowSpan={2}>
            Số đã đưa vào ứng dụng
          </th>
        </tr>
        <tr>
          <th style={{...tblstyle.th, width: 100}}>Số phê duyệt mới trong năm</th>
          <th style={{...tblstyle.th, width: 100}}>Số chuyển tiếp từ năm trước</th>
        </tr>
        <tr>
          <td style={{...tblstyle.td, textAlign: "center"}}>A</td>
          <td style={{...tblstyle.td, textAlign: "center"}}>B</td>
          <td style={{...tblstyle.td, textAlign: "center"}}>1</td>
          <td style={{...tblstyle.td, textAlign: "center"}}>2</td>
          <td style={{...tblstyle.td, textAlign: "center"}}>3</td>
          <td style={{...tblstyle.td, textAlign: "center"}}>4</td>
          <td style={{...tblstyle.td, textAlign: "center"}}>5</td>
          <td style={{...tblstyle.td, textAlign: "center"}}>6</td>
        </tr>
        {data.map((item, index) => (
          <tbody>
          <tr style={{...tblstyle.tr}}>
            <td style={{...tblstyle.td, textAlign: "left"}}>
              <b>
                {index + 1}. {item.HangMuc}
              </b>
            </td>
            <td style={{...tblstyle.td, textAlign: "center"}}/>
            <td style={{...tblstyle.td, textAlign: "center"}}/>
            <td style={{...tblstyle.td, textAlign: "center"}}/>
            <td style={{...tblstyle.td, textAlign: "center"}}/>
            <td style={{...tblstyle.td, textAlign: "center"}}/>
            <td style={{...tblstyle.td, textAlign: "center"}}/>
            <td style={{...tblstyle.td, textAlign: "center"}}/>
          </tr>
          {item.NhiemVuKhoaHoc.map((chitiet) => (
            <tr style={{...tblstyle.tr}}>
              <td style={{...tblstyle.td, textAlign: "left"}}>{chitiet.HangMuc}</td>
              <td style={{...tblstyle.td, textAlign: "center"}}>{chitiet.MaSo}</td>
              <td style={{...tblstyle.td, textAlign: "center"}}>
                {chitiet.TongSo != 0 ? chitiet.TongSo : ""}</td>
              <td style={{...tblstyle.td, textAlign: "center"}}>
                {chitiet.SoChuNhiemLaNu != 0 ? chitiet.SoChuNhiemLaNu : ""}
              </td>
              <td style={{...tblstyle.td, textAlign: "center"}}>
                {chitiet.SoPheDuyet != 0 ? chitiet.SoPheDuyet : ""}
              </td>
              <td style={{...tblstyle.td, textAlign: "center"}}>
                {chitiet.SoChuyenTiep != 0 ? chitiet.SoChuyenTiep : ""}
              </td>
              <td style={{...tblstyle.td, textAlign: "center"}}>
                {chitiet.SoDuocNghiemThu != 0 ? chitiet.SoDuocNghiemThu : ""}
              </td>
              <td style={{...tblstyle.td, textAlign: "center"}}>
                {chitiet.SoDaUngDung != 0 ? chitiet.SoDaUngDung : ""}
              </td>
            </tr>
          ))}
          </tbody>
        ))}
      </table>
    );
  }
}
