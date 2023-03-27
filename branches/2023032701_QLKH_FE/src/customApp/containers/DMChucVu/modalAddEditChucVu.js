import React, {Component} from 'react';
import {MODAL_NORMAL, ITEM_LAYOUT3, REQUIRED} from '../../../settings/constants';
import {Modal, Form, Input, Button} from 'antd';

const {Item} = Form;

const ModalAddEdit = Form.create({name: 'modal_add_adTemplate'})(
  // eslint-disable-next-line
  class extends Component {
    constructor(props) {
      super(props);
      this.state = {};
    }

    componentDidMount() {
      //...
    }

    onOk = (e) => {
      e.preventDefault();
      this.props.form.validateFields((err, value) => {
        if (!err) {
          const {onCreate} = this.props;
          onCreate(value);
        }
      });
    };

    render() {
      const {visible, onCancel, form, dataEdit, loading} = this.props;
      const {getFieldDecorator} = form;
      return (
        <Modal
          title={dataEdit.ChucVuID ? "Sửa thông tin chức vụ" : "Thêm thông tin chức vụ"}
          width={MODAL_NORMAL}
          visible={visible}
          onCancel={onCancel}
          footer={[
            <Button key="back" onClick={onCancel}>Hủy</Button>,
            <Button key="submit" htmlType="submit" type="primary" form="myForm"
                    onClick={this.onOk} disabled={loading} loading={loading}>Lưu</Button>,
          ]}
        >
          <Form id="myForm" layout="horizontal">
            {getFieldDecorator('ChucVuID', {initialValue: dataEdit.ChucVuID ? dataEdit.ChucVuID : null})}
            <Item label="Tên chức vụ" {...ITEM_LAYOUT3}>
              {getFieldDecorator('TenChucVu', {
                initialValue: dataEdit.TenChucVu ? dataEdit.TenChucVu : "",
                rules: [
                  REQUIRED,
                  {
                    max: 50,
                    message: "Tên chức vụ không được quá 50 ký tự",
                  }
                ],
              })(<Input autoFocus/>)}
            </Item>
            <Item label="Ghi chú" {...ITEM_LAYOUT3}>
              {getFieldDecorator('GhiChu', {initialValue: dataEdit.GhiChu ? dataEdit.GhiChu : ""})(<Input.TextArea/>)}
            </Item>
          </Form>
        </Modal>
      );
    }
  },
);
export {ModalAddEdit}