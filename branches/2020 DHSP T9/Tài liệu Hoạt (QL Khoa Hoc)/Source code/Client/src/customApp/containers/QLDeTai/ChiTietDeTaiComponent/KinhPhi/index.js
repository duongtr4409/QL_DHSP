import React from "react";
import moment from "moment";
import { Table, Icon, Button, Modal, message } from "antd";
import ModalComponent from "./modal";
import api from "../../config";
import numeral from "numeral";

export default class TableData extends React.Component {
  modal = null;
  componentDidMount() {}
  openModal = (data = null) => {
    this.modal = Modal.confirm({
      icon: <i />,
      content: <ModalComponent DeTaiID={this.props.DeTaiID} data={data} onClose={this.handleCloseModal}></ModalComponent>,
    });
  };
  handleCloseModal = () => {
    this.props.refresh();
    this.modal.destroy();
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
        title: "Lần",
        dataIndex: "index",
        key: "index",
        render: (text, record, index) => <p>{index + 1}</p>,
      },

      {
        title: "Tổng kinh phí được duyệt",
        dataIndex: "TongKinhPhiDuocDuyet",
        key: "TongKinhPhiDuocDuyet",
        render: (text, record, index) => numeral(text).format("0,0"),
      },
      {
        title: "Tiến độ cấp",
        dataIndex: "TienDoCap",
        key: "TienDoCap",
        render: (text, record, index) => numeral(text).format("0,0"),
      },
      {
        title: "Tiến độ quyết toán",
        dataIndex: "TienDoQuyetToan",
        key: "TienDoQuyetToan",
        render: (text, record, index) => numeral(text).format("0,0"),
      },
      {
        title: "Thao tác",
        dataIndex: "NoiDung",
        key: "NoiDung",

        render: (text, record, index) =>
          record.NguoiTaoID !== JSON.parse(localStorage.getItem("user")).CanBoID ? null : (
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
          ),
      },
    ];

    return (
      <div>
        <div className="mb-2 text-right">
          <Button type="primary" onClick={() => this.openModal(null)} disabled={!this.props.role.add}>
            Thêm thông tin kinh phí
          </Button>
        </div>
        <Table locale={{ emptyText: "Không có dữ liệu" }} pagination={false} rowKey="ChiTietDeTaiID" bordered dataSource={data} columns={columns}></Table>
      </div>
    );
  }
}
