import React, { Component } from "react";
import Constants, { MODAL_NORMAL, ITEM_LAYOUT3, REQUIRED } from "../../../settings/constants";
import { Modal, Form, Input, Radio, Button, message } from "antd";
import TreeSelect from "../../../components/uielements/treeSelect";
import api from "./config";
const { Item } = Form;

const ModalAddGroup = Form.create({ name: "modal_add_adTemplate" })(
  // eslint-disable-next-line
  class extends Component {
    constructor(props) {
      super(props);
      this.state = {
        allRight: false,
        confirmLoading: false,
      };
    }
    componentDidMount() {
      this.setState({
        allRight: true,
      });
    }
    onOk = (e) => {
      e.preventDefault();
      this.props.form.validateFields((err, value) => {
        if (!err) {
          let DanhSachCoQuanID = [];
          if (this.state.applyType !== "1") {
            DanhSachCoQuanID = value.DanhSachCoQuanID;
          }
          value = { ...value, DanhSachCoQuanID };
          this.setState({ confirmLoading: true }, () => {
            api
              .themNhom(value)
              .then((response) => {
                if (response.data.Status > 0) {
                  //message success
                  message.success("Thêm thành công");

                  this.props.onCancel();
                } else {
                  Modal.error({
                    title: "Lỗi",
                    content: response.data.Message,
                  });
                  this.setState({ confirmLoading: false });
                }
              })
              .catch((error) => {
                Modal.error(Constants.API_ERROR);
                this.setState({ confirmLoading: false });
              });
          });
        }
      });
    };

    render() {
      if (!this.state.allRight) return null;
      const { visible, onCancel, form } = this.props;
      const { confirmLoading } = this.state;
      const { getFieldDecorator } = form;
      return (
        <Modal
          title="Thêm thông tin nhóm người dùng"
          width={MODAL_NORMAL}
          visible={true}
          onCancel={onCancel}
          // onOk={()}
          footer={[
            <Button key="back" onClick={onCancel}>
              Hủy
            </Button>,
            <Button key="submit" htmlType="submit" type="primary" form="myForm" loading={confirmLoading} disabled={confirmLoading} onClick={this.onOk}>
              Lưu
            </Button>,
          ]}
        >
          <Form id="myForm" layout="horizontal">
            <Item label="Tên nhóm người dùng" {...ITEM_LAYOUT3}>
              {getFieldDecorator("TenNhom", {
                rules: [{ ...REQUIRED }],
              })(<Input autoFocus />)}
            </Item>

            <Item label="Ghi chú" {...ITEM_LAYOUT3}>
              {getFieldDecorator("GhiChu")(<Input.TextArea />)}
            </Item>
          </Form>
        </Modal>
      );
    }
  }
);
export { ModalAddGroup };
