import React from "react";
import moment from "moment";
import { Table, Icon, Button, Modal, message } from "antd";
import ModalComponent from "./modal";
import api from "../../config";

// {
//   title: "File đính kèm",
//   dataIndex: "files",
//   key: "files",
//   render: (text, record, index) => (
//     <div>
//       {record.files.map((item) => (
//         <span className="border border-primary rounded mx-1 my-1 d-inline-block p-1">
//           <a download={record.TenFileGoc} target="_blank" href={record.FileUrl}>
//             {record.TenFileGoc}
//           </a>
//           {/* <Icon onClick={() => this.onDeleteFile(item)} className="pointer ml-1" type="close"></Icon> */}
//         </span>
//       ))}
//     </div>
//   ),
// },

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
        title: "STT",
        dataIndex: "index",
        key: "index",
        render: (text, record, index) => <p>{index + 1}</p>,
      },

      {
        title: "Tên nội dung",
        dataIndex: "TenNoiDung",
        key: "TenNoiDung",
      },
      {
        title: "Ngày thực hiện",
        dataIndex: "NgayThucHien",
        key: "NgayThucHien",
        render: (text, record, index) => <p>{text ? moment(text).format("DD/MM/YYYY") : ""}</p>,
      },

      {
        title: "Kết quả",
        dataIndex: "KetQuaDatDuoc",
        key: "KetQuaDatDuoc",
      },
      {
        title: "Ghi chú",
        dataIndex: "GhiChu",
        key: "GhiChu",
      },
      {
        title: "Trạng thái",
        dataIndex: "TrangThaiTienDo",
        key: "TrangThaiTienDo",
        render: (text, record, index) => <p>{record.TrangThaiTienDo === 0 ? "Chưa thực hiện" : record.TrangThaiTienDo === 1 ? "Đang thực hiện" : "Đã thực hiện"}</p>,
      },
      {
        title: "Thao tác",
        dataIndex: "NoiDung",
        key: "NoiDung",

        render: (text, record, index) => (
          // record.NguoiTaoID !== JSON.parse(localStorage.getItem("user")).CanBoID ? null : (
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

            {/* <span>
                <Button
                  disabled={!this.props.role.delete}
                  onClick={() => {
                    this.confirmDelete(record);
                  }}
                  className="mx-2 pointer"
                  icon="close"
                ></Button>
              </span> */}
          </div>
        ),
        // ),
      },
    ];

    return (
      <div>
        {/* <div className="mb-2 text-right">
          <Button type="primary" onClick={() => this.openModal(null)} disabled={!this.props.role.add}>
            Cập nhật tiến độ
          </Button>
        </div> */}
        <Table locale={{ emptyText: "Không có dữ liệu" }} pagination={false} rowKey="ChiTietDeTaiID" bordered dataSource={data} columns={columns}></Table>
      </div>
    );
  }
}
