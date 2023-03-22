import React, {Component} from 'react';
import api from "./config";
import Constants, {MODAL_NORMAL, ITEM_LAYOUT3, REQUIRED} from '../../settings/constants';
import {Modal, Form, Input, Radio, Button, message} from 'antd';

const {Item} = Form;

const ModalChangePassword = Form.create({name: 'modal_change_password'})(
  // eslint-disable-next-line
  class extends Component {
    constructor(props) {
      super(props);
      this.state = {
        visibleLocal: true,
        loading: false
      };
    }

    onOk = (e) => {
      e.preventDefault();
      this.props.form.validateFields((err, value) => {
        if (!err) {
          if (value.OldPassword === value.NewPassword) {
            Modal.error({
              title: "Thông báo",
              content: "Mật khẩu mới không được giống mật khẩu cũ"
            })
          } else if (value.NewPassword.indexOf(" ") >= 0) {
            Modal.error({
              title: "Thông báo",
              content: "Mật khẩu không được chứa khoảng trắng"
            })
          } else if (value.NewPassword !== value.ConfirmPassword) {
            Modal.error({
              title: "Thông báo",
              content: "Mật khẩu mới không trùng với nhập lại mật khẩu"
            })
          } else {
            this.setState({loading: true}, () => {
              api.changePassword(value)
                .then(response => {
                  this.setState({loading: false}, () => {
                    if (response.data.Status > 0) {
                      message.success("Cập nhật mật khẩu thành công");
                      this.onCancelLocal();
                      setTimeout(() => {
                        this.props.logout();
                      }, 1000);
                    } else {
                      Modal.error({
                        title: "Lỗi",
                        content: response.data.Message
                      });
                    }
                  });
                }).catch(error => {
                Modal.error(Constants.API_ERROR)
              });
            });
          }
        }
      });
    };
    onCancelLocal = () => {
      this.setState({visibleLocal: false}, () => {
        this.props.onCancel();
      });
    };

    render() {
      const {form} = this.props;
      const {getFieldDecorator} = form;
      let visible = false;
      if (this.props.visible && this.state.visibleLocal) {
        visible = true;
      }
      return (
        <Modal
          title="Thay đổi mật khẩu"
          width={650}
          visible={visible}
          onCancel={this.onCancelLocal}
          footer={[
            <Button key="back" onClick={this.onCancelLocal}>Hủy</Button>,
            <Button key="submit" htmlType="submit" type="primary" form="myForm"
                    loading={this.state.loading} onClick={this.onOk}>Cập nhật</Button>,
          ]}
        >
          <Form id="myForm" layout="horizontal">
            <Item label="Mật khẩu hiện tại" {...ITEM_LAYOUT3}>
              {getFieldDecorator('OldPassword', {
                rules: [REQUIRED],
              })(<Input.Password autoFocus/>)}
            </Item>
            <Item label="Mật khẩu mới" {...ITEM_LAYOUT3}>
              {getFieldDecorator('NewPassword', {
                rules: [
                  REQUIRED,
                  {
                    min: 6,
                    message: "Mật khẩu của bạn quá ngắn",
                  },
                  {
                    max: 30,
                    message: "Mật khẩu của bạn quá dài",
                  }
                ],
              })(<Input.Password/>)}
            </Item>
            <Item label="Nhập lại mật khẩu mới" {...ITEM_LAYOUT3}>
              {getFieldDecorator('ConfirmPassword', {
                rules: [REQUIRED],
              })(<Input.Password/>)}
            </Item>
          </Form>
        </Modal>
      );
    }
  },
);
export {ModalChangePassword}