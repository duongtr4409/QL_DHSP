import React from "react";
import { Modal, Icon, Button, Input, Upload, message, Table, Popover } from "antd";
import moment from "moment";
import api from "./config";
import { isArray } from "lodash";
let modal = null;
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
export default class GoModal extends React.PureComponent {
  renderFile = (files) => {};
  renderTableRaw = (key, obj) => {
    if (obj[key].toString().includes("</p>")) {
      return <p dangerouslySetInnerHTML={{ __html: obj[key] }} />;
    }
    if (["CapQuanLy", "LinhVucKinhTeXaHoi", "LinhVucNghienCuu", "NguoiDeXuat"].includes(key)) {
      return obj[`Ten${key}`];
    }
    if (["NgayDeXuat", "NgayThucHien"].includes(key)) {
      return moment(obj[key]).format("DD/MM/YYYY");
    }

    return obj[`${key}`];
  };

  renderCompare = (key, data) => {
    return (
      <Modal
        visible={true}
        width={960}
        footer={null}
        title={"Chi tiết"}
        onCancel={() => {
          modal.destroy();
        }}
      >
        <div>
          <table className="w-100" style={tblstyle.table}>
            <thead>
              <tr className="text-center" style={tblstyle.tr}>
                <th style={tblstyle.th}>Nội dung trước khi chỉnh sửa</th>
                <th style={tblstyle.th}>Nội dung sau khi chỉnh sửa</th>
              </tr>
            </thead>
            <tbody>
              {isArray(data[0][key]) ? (
                <tr>
                  <td style={tblstyle.td}>{data[0][key].map((item) => item.TenFileGoc).join(", ")}</td>
                  <td style={tblstyle.td}>{data[1][key].map((item) => item.TenFileGoc).join(", ")}</td>
                </tr>
              ) : (
                <tr className="text-center" style={tblstyle.tr}>
                  <td style={tblstyle.td}>{this.renderTableRaw(key, data[0])}</td>
                  <td style={tblstyle.td}>{this.renderTableRaw(key, data[1])}</td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
      </Modal>
    );
  };
  render() {
    const columns = [
      {
        title: "STT",
        dataIndex: "index",
        key: "index",
        width: 50,
        render: (text, record, index) => <p>{index + 1}</p>,
      },
      {
        title: "Cán bộ thực hiện",
        dataIndex: "NguoiChinhSua",
        key: "NguoiChinhSua",
      },
      {
        title: "Ngày thực hiện",
        dataIndex: "NgayChinhSua",
        key: "NgayChinhSua",
        render: (text, record, index) => <p>{moment(text).format("DD/MM/YYYY")}</p>,
      },
      {
        title: "Các nội dung đã chỉnh sửa",
        dataIndex: "NoiDungChinhSua",
        key: "NoiDungChinhSua",
        render: (text, record, index) => (
          <p>
            {record.NoiDungChinhSua.map((item, index) => {
              return (
                <span
                  onClick={() => {
                    modal = Modal.confirm({
                      style: { maxWidth: "80%" },
                      icon: <i></i>,
                      content: this.renderCompare(item.key, record.Data),
                      maskClosable: true,
                    });
                  }}
                  className="pointer text-primary"
                >{`${item.label}${index === record.NoiDungChinhSua.length - 1 ? "" : ", "}`}</span>
              );
            })}
          </p>
        ),
      },
    ];

    return <Table locale={{ emptyText: "Không có dữ liệu" }} pagination={false} rowKey="ID" bordered dataSource={this.props.data} columns={columns}></Table>;
  }
}
