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

  formatNumberBaoCao = (number) => {
    if (!number) {
      return "";
    }
    number = number.toString();
    const numberArr = number.split(".");
    const firstNum = numeral(numberArr[0]).format("0,0");
    const secondNum = numberArr[1] ? numberArr[1] : "";
    return `${firstNum}${secondNum !== "" ? "." : ""}${secondNum}`;
  };

  render() {
    const data = this.props.data || [];
    const {filter} = this.props;
    const CapQG = data[0] ? data[0].NhiemVuKhoaHoc : [];
    const CapBo = data[1] ? data[1].NhiemVuKhoaHoc : [];
    return (
      <table style={tblstyle.table}>
        <tr style={tblstyle.tr}>
          <th style={{...tblstyle.th, fontSize: 18, border: "none", textAlign: "left"}} colSpan={9}>
            TRƯỜNG ĐẠI HỌC SƯ PHẠM HÀ NỘI
          </th>
        </tr>
        <tr style={tblstyle.tr}>
          <th style={{...tblstyle.th, fontSize: 18, border: "none"}} colSpan={9}>
            TÌNH HÌNH VÀ KẾT QUẢ THỰC HIỆN NHIỆM VỤ KHOA HỌC VÀ CÔNG NGHỆ CẤP QUỐC GIA VÀ CẤP BỘ TRONG NĂM TRƯỚC NĂM BÁO
            CÁO VÀ NĂM BÁO CÁO
          </th>
        </tr>
        <tr style={tblstyle.tr}>
          <th style={{...tblstyle.th, fontSize: 13, border: "none"}} colSpan={9}>
            (Kèm theo Báo cáo số ... ngày ... tháng ... năm ...)
          </th>
        </tr>
        <tr/>

        <tr style={tblstyle.tr}>
          <th rowSpan="2" style={{...tblstyle.th, width: 50}}>
            STT
          </th>
          <th rowSpan="2" style={{...tblstyle.th, width: 350}}>
            Tên nhiệm vụ
          </th>
          <th rowSpan="2" style={{...tblstyle.th, width: 250}}>
            Đơn vị chủ trì, chủ nhiệm
          </th>
          <th rowSpan="2" style={{...tblstyle.th, width: 150}}>
            Thời gian thực hiện
          </th>
          <th colSpan="3" scope="colgroup" style={{...tblstyle.th, width: 300}}>
            Kinh phí (triệu đồng)
          </th>
          <th rowSpan="2" style={{...tblstyle.th, width: 350}}>
            Kết quả đạt được
          </th>
          <th rowSpan="2" style={{...tblstyle.th, width: 200}}>
            Ghi chú
          </th>
        </tr>
        <tr style={tblstyle.tr}>
          <th scope="col" style={{...tblstyle.th, width: 100}}>
            Từ NSNN
          </th>
          <th scope="col" style={{...tblstyle.th, width: 100}}>
            Nguồn khác
          </th>
          <th scope="col" style={{...tblstyle.th, width: 100}}>
            Số đã cập đến tháng 6 năm báo cáo
          </th>
        </tr>

        <tbody>
        <tr style={tblstyle.tr}>
          <td style={{...tblstyle.td, textAlign: "center", fontWeight: 'bold'}}>I</td>
          <td style={{...tblstyle.td, textAlign: "center", fontWeight: 'bold'}}>Nhiệm vụ KH & CN cấp Quốc gia</td>
          <td style={{...tblstyle.td, textAlign: "center"}}/>
          <td style={{...tblstyle.td, textAlign: "center"}}/>
          <td style={{...tblstyle.td, textAlign: "center"}}/>
          <td style={{...tblstyle.td, textAlign: "center"}}/>
          <td style={{...tblstyle.td, textAlign: "center"}}/>
          <td style={{...tblstyle.td, textAlign: "center"}}/>
          <td style={{...tblstyle.td, textAlign: "center"}}/>
        </tr>
        {CapQG.map((item, index) => (
          <tr style={tblstyle.tr}>
            <td style={{...tblstyle.td, textAlign: "center"}}>{index + 1}</td>
            <td style={{...tblstyle.td, textAlign: "left"}}>{item.TenNhiemVu}</td>
            <td style={{...tblstyle.td, textAlign: "left"}}>{item.DonViChuTri}</td>
            <td style={{...tblstyle.td, textAlign: "center"}}>{item.ThoiGianThucHien}</td>
            <td style={{
              ...tblstyle.td,
              textAlign: "center"
            }}>{item.KinhPhiNSNN != 0 ? this.formatNumberBaoCao(item.KinhPhiNSNN) : ""}</td>
            <td style={{
              ...tblstyle.td,
              textAlign: "center"
            }}>{item.NguonKhac != 0 ? this.formatNumberBaoCao(item.NguonKhac) : ""}</td>
            <td style={{
              ...tblstyle.td,
              textAlign: "center"
            }}>{item.SoDaCap != 0 ? this.formatNumberBaoCao(item.SoDaCap) : ""}</td>
            <td style={{...tblstyle.td, textAlign: "left"}}>{item.KetQuaDatDuoc}</td>
            <td style={{...tblstyle.td, textAlign: "left"}}>{item.GhiChu}</td>
          </tr>
        ))}
        <tr style={tblstyle.tr}>
          <td style={{...tblstyle.td, textAlign: "center", fontWeight: 'bold'}}>II</td>
          <td style={{...tblstyle.td, textAlign: "center", fontWeight: 'bold'}}>Nhiệm vụ KH & CN cấp Bộ</td>
          <td style={{...tblstyle.td, textAlign: "center"}}/>
          <td style={{...tblstyle.td, textAlign: "center"}}/>
          <td style={{...tblstyle.td, textAlign: "center"}}/>
          <td style={{...tblstyle.td, textAlign: "center"}}/>
          <td style={{...tblstyle.td, textAlign: "center"}}/>
          <td style={{...tblstyle.td, textAlign: "center"}}/>
          <td style={{...tblstyle.td, textAlign: "center"}}/>
        </tr>
        {CapBo.map((item, index) => (
          <tr style={tblstyle.tr}>
            <td style={{...tblstyle.td, textAlign: "center"}}>{index + 1}</td>
            <td style={{...tblstyle.td, textAlign: "left"}}>{item.TenNhiemVu}</td>
            <td style={{...tblstyle.td, textAlign: "left"}}>{item.DonViChuTri}</td>
            <td style={{...tblstyle.td, textAlign: "center"}}>{item.ThoiGianThucHien}</td>
            <td style={{
              ...tblstyle.td,
              textAlign: "center"
            }}>{item.KinhPhiNSNN != 0 ? this.formatNumberBaoCao(item.KinhPhiNSNN) : ""}</td>
            <td style={{
              ...tblstyle.td,
              textAlign: "center"
            }}>{item.NguonKhac != 0 ? this.formatNumberBaoCao(item.NguonKhac) : ""}</td>
            <td style={{
              ...tblstyle.td,
              textAlign: "center"
            }}>{item.SoDaCap != 0 ? this.formatNumberBaoCao(item.SoDaCap) : ""}</td>
            <td style={{...tblstyle.td, textAlign: "left"}}>{item.KetQuaDatDuoc}</td>
            <td style={{...tblstyle.td, textAlign: "left"}}>{item.GhiChu}</td>
          </tr>
        ))}
        </tbody>
      </table>
    );
  }
}
