import React from "react";
import moment from "moment";
import { Table, Icon, Button, Modal, message, Input } from "antd";

import ModalComponent from "./modal";
import api from "../../config";
import apiConfig from "../../../ThamSoHeThong/config";

export default class TableData extends React.Component {
  modal = null;
  state = {
    data: null,
    tableData: [],
    LoaiKetQua: null,
    canEdit: false,
  };
  componentDidMount() {
    apiConfig.GetByKey({ ConfigKey: "ID_NHOM_QUYEN_QLKH" }).then((res) => {
      if (res.data.Data.ConfigValue === localStorage.getItem("role_id")) {
        this.setState({ canEdit: true });
      }
    });
  }
  componentWillReceiveProps(props) {
    if (props.data && props.data.length !== 0 && props.data !== this.props.data) {
      if (props.data[0].LoaiKetQua === 1) {
        this.setState({ LoaiKetQua: 1, tableData: props.data, data: null });
      } else {
        this.setState({ LoaiKetQua: 0, tableData: props.data, data: null });
      }
    }
    if (props.data && props.data.length === 0 && props.data !== this.props.data) {
      this.setState({ tableData: props.data, data: null });
    }
  }

  handleCloseModal = () => {
    this.props.refresh();
  };
  confirmDelete = (record) => {
    Modal.confirm({
      content: "Bạn có muốn xoá dữ liệu này?",
      cancelText: "Hủy",
      okText: "Xoá",
      onOk: () => {
        api.xoaThongTinChiTiet({ ChiTietDeTaiID: record.ChiTietDeTaiID }).then((res) => {
          if (!res || !res.data || res.data.Status !== 1) {
            this.setState({ confirmLoading: false });
            message.error(`${res && res.data ? res.data.Message : "Lỗi hệ thống"}`);
          } else {
            message.success("Xoá thành công");
            this.props.refresh();
          }
        });
      },
    });
  };
  render() {
    const data = this.props.data;
    const { DeTaiID } = this.props;
    const columns = [
      {
        title: "STT",
        dataIndex: "index",
        key: "index",
        render: (text, record, index) => <p>{index + 1}</p>,
      },
      {
        title: "Ngày nghiệm thu",
        dataIndex: "NgayNghiemThu",
        key: "NgayNghiemThu",
        render: (text, record, index) => <p>{text ? moment(text).format("DD/MM/YYYY") : ""}</p>,
      },
      {
        title: "Loại nghiệm thu",
        dataIndex: "LoaiNghiemThu",
        key: "LoaiNghiemThu",
        render: (text, record, index) => <p>{record.LoaiKetQua === 0 ? (text === 0 ? "Nghiệm thu cơ sở" : "Nghiệm thu chính thức") : "Thanh lý"}</p>,
      },
      {
        title: "Xếp loại",
        dataIndex: "XepLoai",
        key: "XepLoai",
        render: (text, record, index) => {
          if (record.LoaiKetQua === 1) {
            return "";
          }
          if (text === 0) {
            return "Không đạt";
          }
          if (text === 1) {
            return "Đạt";
          }
          if (text === 2) {
            return "Xuất sắc";
          }
          if (text === 3) {
            return record.XepLoaiKhac;
          }
        },
      },

      {
        title: "Quyết định",
        dataIndex: "QuyetDinh",
        key: "QuyetDinh",
      },
      {
        title: "File đính kèm",
        dataIndex: "files",
        key: "files",
        render: (text, record, index) => (
          <div>
            {record.FileDinhKem.map((item, index) => (
              <span className="border border-primary rounded mx-1 my-1 d-inline-block p-1" key={index}>
                <a download={item.TenFileGoc} target="_blank" href={item.FileUrl}>
                  {item.TenFileGoc}
                </a>
                {/* <Icon onClick={() => this.onDeleteFile(item)} className="pointer ml-1" type="close"></Icon> */}
              </span>
            ))}
          </div>
        ),
      },
    ];
    if (this.state.canEdit) {
      columns.push({
        title: "Thao tác",
        dataIndex: "NoiDung",
        key: "NoiDung",
        width: "150px",
        render: (text, record, index) =>
          record.NguoiTaoID !== JSON.parse(localStorage.getItem("user")).CanBoID ? null : (
            <div className="text-center">
              <span className="mx-2 pointer">
                <Button
                  disabled={!this.props.role.edit}
                  onClick={() => {
                    const firstError = document.getElementById("KQDG");
                    if (firstError) {
                      firstError.scrollIntoView({ behavior: "smooth", block: "end", inline: "nearest" });
                    }
                    this.setState({ data: record });
                  }}
                  icon="edit"
                ></Button>
              </span>

              <span>
                <Button
                  disabled={!this.props.role.delete}
                  onClick={() => {
                    this.confirmDelete(record);
                  }}
                  className="mx-2 pointer"
                  icon="close"
                ></Button>
              </span>
            </div>
          ),
      });
    }

    return (
      <div>
        {this.state.canEdit ? (
          this.state.LoaiKetQua !== 1 ? (
            <ModalComponent data={this.state.data} LoaiKetQua={this.state.LoaiKetQua} onClose={this.handleCloseModal} DeTaiID={DeTaiID}></ModalComponent>
          ) : this.state.data ? (
            <ModalComponent data={this.state.data} LoaiKetQua={this.state.LoaiKetQua} onClose={this.handleCloseModal} DeTaiID={DeTaiID}></ModalComponent>
          ) : (
            ""
          )
        ) : (
          ""
        )}
        {/* {!this.state.canEdit && this.state.LoaiKetQua === 1 ? (
          <div className="row">
            <div className="col-2">Kết quả</div>
            <div className="col-8"></div>
            <div className="col-2">Ngày nghiệm thu</div>
            <div className="col-8"></div>
            <div className="col-2">Quyết định</div>
            <div className="col-8"></div>
            <div className="col-2">File đính kèm</div>
            <div className="col-8"></div>
          </div>
        ) : (
          ""
        )} */}
        {/* {!this.state.LoaiKetQua ? ( */}
        <Table locale={{ emptyText: "Không có dữ liệu" }} pagination={false} rowKey="ChiTietDeTaiID" bordered dataSource={this.state.tableData} columns={columns}></Table>
        {/* ) : (
          ""
        )} */}
      </div>
    );
  }
}
