import React, { Component } from "react";
import Constants, { MODAL_NORMAL, ITEM_LAYOUT3, REQUIRED } from "../../../settings/constants";
import { Modal, Form, Input, Button } from "antd";
import Select, { Option } from "../../../components/uielements/select";
import api from "./config";
const { Item } = Form;

const ModalAddUser = Form.create({ name: "modal_add_adTemplate" })(
  // eslint-disable-next-line
  class extends Component {
    constructor(props) {
      super(props);
      this.state = {
        allRight: false,
        NhomNguoiDungID: 0,
        NguoiDungID: null,
        CoQuanID: null,
        DanhSachNguoiDung: [],
        DanhSachCoQuan: [],
      };
    }
    componentDidMount() {
      const { NhomNguoiDungID, DanhSachNguoiDung } = this.props.dataModalAddUser;
      this.setState({
        allRight: NhomNguoiDungID,
        NhomNguoiDungID,
        DanhSachNguoiDung,
      });
      api.danhSachCoQuan().then((res) => {
        if (res.status !== 200 || !res.data) {
          Modal.error({
            title: "Lỗi",
            content: `${res.status} - ${res.statusText}`,
          });
        } else {
          if (res.data.Status !== 1) {
            Modal.error({
              title: "Lỗi",
              content: res.data.Message,
            });
          } else {
            this.setState({ DanhSachCoQuan: res.data.Data });
          }
        }
      });
    }
    onOk = (e) => {
      e.preventDefault();
      this.props.form.validateFields((err, value) => {
        if (!err) {
          const { onCreate } = this.props;
          onCreate(value);
        }
      });
    };

    onSelectCoQuan = (value) => {
      this.state.CoQuanID = value;
      api.danhSachNguoiDungTheoCoQuanIDvaNhomNguoiDungID({ CoQuanID: value, NhomNguoiDungID: this.props.dataModalAddUser.NhomNguoiDungID }).then((res) => {
        if (res.status !== 200 || !res.data) {
          Modal.error({
            title: "Lỗi",
            content: `${res.status} - ${res.statusText}`,
          });
        } else {
          if (res.data.Status !== 1) {
            Modal.error({
              title: "Lỗi",
              content: res.data.Message,
            });
          } else {
            this.props.form.setFieldsValue({ NguoiDungID: null });
            this.setState({ DanhSachNguoiDung: res.data.Data });
          }
        }
      });
    };
    render() {
      if (!this.state.allRight) return null;
      const { confirmLoading, visible, onCancel, form } = this.props;
      // console.log(this.props);
      const { getFieldDecorator } = form;
      return (
        <Modal
          maskClosable={false}
          title="Thêm người dùng vào nhóm"
          width={MODAL_NORMAL}
          visible={visible}
          onCancel={onCancel}
          footer={[
            <Button key="back" onClick={onCancel}>
              Hủy
            </Button>,
            <Button key="submit" htmlType="submit" type="primary" form="myForm" loading={confirmLoading} onClick={this.onOk}>
              Lưu
            </Button>,
          ]}
        >
          <Form id="myForm" layout="horizontal">
            <Item style={{ display: "none" }}>
              {getFieldDecorator("CoQuanID", {
                initialValue: this.state.CoQuanID,
              })(<Input />)}
            </Item>
            <Item label="Chọn cơ quan" {...ITEM_LAYOUT3}>
              {getFieldDecorator("CoQuanID", {
                rules: [{ ...REQUIRED }],
              })(
                <Select onSelect={this.onSelectCoQuan} showSearch placeholder="Chọn cơ quan">
                  {this.state.DanhSachCoQuan.map((value) => (
                    <Option key={value.CoQuanID} value={value.CoQuanID}>
                      {`${value.TenCoQuan}`}
                    </Option>
                  ))}
                </Select>
              )}
            </Item>
            <Item style={{ display: "none" }}>
              {getFieldDecorator("NhomNguoiDungID", {
                initialValue: this.state.NhomNguoiDungID,
              })(<Input />)}
            </Item>
            <Item label="Chọn người dùng" {...ITEM_LAYOUT3}>
              {getFieldDecorator("NguoiDungID", {
                rules: [{ ...REQUIRED }],
              })(
                <Select showSearch placeholder="Chọn người dùng">
                  {this.state.DanhSachNguoiDung.map((value) => (
                    <Option key={value.NguoiDungID} value={value.NguoiDungID}>
                      {`${value.TenNguoiDung}`}
                    </Option>
                  ))}
                </Select>
              )}
            </Item>
          </Form>
        </Modal>
      );
    }
  }
);
export { ModalAddUser };
