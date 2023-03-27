import React from "react";
import moment from "moment";
import { Table, Icon, Button, Modal, message } from "antd";
import ModalComponent from "./modal";
import api from "../../config";
import apiConfig from "../../../ThamSoHeThong/config";

export default class TableData extends React.Component {
  state = {
    canEdit: false,
  };
  modal = null;
  user = JSON.parse(localStorage.getItem("user"));
  componentDidMount() {
    apiConfig.GetByKey({ ConfigKey: "ID_NHOM_QUYEN_QLKH" }).then((res) => {
      if (res.data.Data.ConfigValue === localStorage.getItem("role_id")) {
        this.setState({ canEdit: true });
      }
    });
  }
  openModal = (data = null) => {
    this.modal = Modal.confirm({
      icon: <i />,
      content: <ModalComponent DeTaiID={this.props.DeTaiID} data={data} onClose={this.handleCloseModal}></ModalComponent>,
    });
  };
  handleCloseModal = () => {
    this.modal.destroy();
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
    const columns = [
      {
        title: "STT",
        dataIndex: "index",
        key: "index",
        render: (text, record, index) => <p>{index + 1}</p>,
      },
      {
        title: "Ngày đánh giá",
        dataIndex: "ThoiGianThucHien",
        key: "ThoiGianThucHien",
        render: (text, record, index) => <p>{moment(text).format("DD/MM/YYYY")}</p>,
      },
      {
        title: "Kết quả thực hiện",
        dataIndex: "KetQuaThucHien",
        key: "KetQuaThucHien",
      },
      {
        title: "Ghi chú",
        dataIndex: "NoiDungDanhGia",
        key: "NoiDungDanhGia",
      },
      {
        title: "Báo cáo đính kèm",
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
        render: (text, record, index) => {
          return record.NguoiTaoID !== JSON.parse(localStorage.getItem("user")).CanBoID ? null : (
            <div className="text-center">
              <span className="mx-2 pointer">
                <Button
                  disabled={!this.props.role.edit}
                  onClick={() => {
                    this.openModal(record);
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
          );
        },
      });
    }

    return (
      <div>
        <div className="mb-2 text-right">
          {this.state.canEdit ? (
            <Button type="primary" onClick={() => this.openModal(null)} disabled={!this.props.role.add}>
              Thêm
            </Button>
          ) : (
            ""
          )}
        </div>
        <Table locale={{ emptyText: "Không có dữ liệu" }} pagination={false} rowKey="ChiTietDeTaiID" bordered dataSource={data} columns={columns}></Table>
      </div>
    );
  }
}
