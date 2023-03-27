import React from "react";
import moment from "moment";
import api from "./config";
import { Button, Checkbox, message } from "antd";
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
    fontFamily: "Times New Roman",
  },
  tr: {
    pageBreakInside: "avoid",
    pageBreakAfter: "always",
  },
};
export default class Report extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      lisPublic: [],
      confirmLoading: false,
    };
    this.formPrint = React.createRef();
  }
  togglePublic = (item) => (event) => {
    const { lisPublic } = this.state;
    const index = lisPublic.findIndex((d) => d.KetQuaNghienCuuID === item.KetQuaNghienCuuID);

    if (index !== -1) {
      lisPublic[index].CongBo = event.target.checked;
    } else {
      lisPublic.push({
        CongBo: event.target.checked,
        KetQuaNghienCuuID: item.KetQuaNghienCuuID,
        CTNhaKhoaHocID: item.CTNhaKhoaHocID,
      });
    }
    this.setState({ lisPublic });
  };
  onPublicResult = () => {
    this.setState({ confirmLoading: true }, () => {
      api.congBoKetQua(this.state.lisPublic).then((res) => {
        if (res && res.data && res.data.Status === 1) {
          message.success("Công bố kết quả thành công");
        } else {
          message.warning("Công bố kết thất bại");
        }
        this.setState({ confirmLoading: false });
      });
    });
  };

  render() {
    const data = this.props.data || [];
    return (
      <div>
        <table style={tblstyle.table}>
          <tr style={tblstyle.tr}>
            <th style={{ ...tblstyle.th, fontSize: 16, border: "none", textAlign: "left" }} colSpan={6}>
              TRƯỜNG ĐẠI HỌC SƯ PHẠM HÀ NỘI
            </th>
            <th style={{ ...tblstyle.th, fontSize: 16, border: "none", textAlign: "center" }} colSpan={2}>
              <p>CỘNG HOÀ XÃ HỘI CHỦ NGHĨA VIỆT NAM</p>
            </th>
          </tr>
          <tr style={tblstyle.tr}>
            <th style={{ ...tblstyle.th, fontSize: 16, border: "none", textAlign: "left" }} colSpan={6}></th>
            <th style={{ ...tblstyle.th, fontSize: 14, border: "none", textAlign: "center" }} colSpan={2}>
              <p>Độc lập - Tự do - Hạnh phúc</p>
            </th>
          </tr>
          <tr style={tblstyle.tr}>
            <th style={{ ...tblstyle.th, fontSize: 16, border: "none" }} colSpan={8}>
              <p style={{ marginTop: 16, marginBottom: 16 }}> BÁO CÁO THỐNG KÊ KẾT QUẢ NGHIÊN CỨU</p>
            </th>
          </tr>

          <tr />
          <tr style={tblstyle.tr}>
            <th style={{ ...tblstyle.th, width: 50 }}>STT</th>
            <th style={{ ...tblstyle.th, width: 100 }}>Tác giả</th>
            <th style={{ ...tblstyle.th, width: 200 }}>Tên công trình khoa học</th>
            <th style={{ ...tblstyle.th, width: 200 }}>Tên tạp chí/sách/hội thảo</th>
            <th style={{ ...tblstyle.th, width: 50 }}>Số</th>
            <th style={{ ...tblstyle.th, width: 50 }}>Trang</th>
            <th style={{ ...tblstyle.th, width: 250 }}>Nhà xuất bản</th>
            <th style={{ ...tblstyle.th, width: 100, textAlign: "center" }}>Thời gian</th>
            {!this.props.exporting && this.props.role.congbo ? <th style={{ ...tblstyle.th, width: 50 }}>Công bố</th> : null}
          </tr>

          {data.map((item, index) => (
            <tr>
              <td style={{ ...tblstyle.td, textAlign: "center" }}>{index + 1}</td>
              <td style={{ ...tblstyle.td, textAlign: "left" }}>{item.TacGia}</td>
              <td style={{ ...tblstyle.td, textAlign: "left" }}>{item.TenCongTrinhKhoaHoc}</td>
              <td style={{ ...tblstyle.td, textAlign: "left" }}>{item.TenTapChiSachHoiThao}</td>
              <td style={{ ...tblstyle.td, textAlign: "center" }}>{item.So}</td>
              <td style={{ ...tblstyle.td, textAlign: "center" }}>{item.Trang}</td>
              <td style={{ ...tblstyle.td, textAlign: "left" }}>{item.NhaXuatBan}</td>
              <td style={{ ...tblstyle.td, textAlign: "center" }}>{item.ThoiGian}</td>
              {!this.props.exporting && this.props.role.congbo ? (
                <td style={{ ...tblstyle.td, width: 50, textAlign: "center" }}>
                  <Checkbox defaultChecked={item.CongBo} onChange={this.togglePublic(item)}></Checkbox>
                </td>
              ) : null}
            </tr>
          ))}
        </table>
        {!this.props.exporting && this.props.role.congbo ? (
          <div className="text-center my-2">
            <Button loading={this.state.confirmLoading} type="primary" onClick={this.onPublicResult}>
              Công bố kết quả
            </Button>
          </div>
        ) : null}
      </div>
    );
  }
}
