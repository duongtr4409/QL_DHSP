import React, {Component} from 'react';
import {Button, Form, Input, message, Modal,} from 'antd';
import Constants, {LoaiThongTinNhaKhoaHoc} from "../../../settings/constants";

const {
  ITEM_LAYOUT3,
} = Constants;
const {Item} = Form;

const ModalAddEditNgoaiNgu = Form.create({name: 'modal_add_edit'})(
  class extends Component {
    constructor(props) {
      super(props);
      this.state = {};
    }

    onOk = (e) => {
      e.preventDefault();
      this.props.form.validateFields((err, value) => {
        if (!err) {
          if (value.TenNgoaiNgu === "" && value.Doc === "" && value.Viet === "" && value.Noi === "") {
            message.destroy();
            message.warning('Không có thông tin được nhập')
            return;
          }
          const {onCreate} = this.props;
          onCreate(value);
        }
      });
    };

    componentDidMount() {

    }

    onCancel = () => {
      const {onCancel} = this.props;
      onCancel();
    };

    render() {
      const {visible, onCancel, form, dataEdit, loading, actions} = this.props;
      const {getFieldDecorator} = form;
      let titleModal = "Thêm thông tin ngoại ngữ";
      if (actions === 'edit') {
        titleModal = "Cập nhật thông tin ngoại ngữ";
      }

      return (
        <Modal
          title={titleModal}
          width={600}
          onCancel={this.onCancel}
          visible={visible}
          footer={[
            <Button key="back" onClick={onCancel}>Hủy</Button>,
            <Button key="submit" htmlType="submit" type="primary" form="formngoaingu"
                    onClick={this.onOk} loading={loading} disabled={loading}>Lưu</Button>,
          ]}
        >
          <Form id="formngoaingu">
            {getFieldDecorator('CTNhaKhoaHocID', {initialValue: dataEdit.CTNhaKhoaHocID ? dataEdit.CTNhaKhoaHocID : null})}
            {getFieldDecorator('CanBoID', {initialValue: this.props.CanBoID})}
            {getFieldDecorator('CoQuanID', {initialValue: this.props.CoQuanID})}
            {getFieldDecorator('LoaiThongTin', {initialValue: LoaiThongTinNhaKhoaHoc.NgoaiNgu})}
            <Item label={"Tên ngoại ngữ"} {...ITEM_LAYOUT3}>
              {getFieldDecorator('TenNgoaiNgu', {
                initialValue: dataEdit.TenNgoaiNgu ? dataEdit.TenNgoaiNgu : ""
              })(
                <Input autoFocus/>)}
            </Item>
            <Item label={"Đọc"} {...ITEM_LAYOUT3}>
              {getFieldDecorator('Doc', {
                initialValue: dataEdit.Doc ? dataEdit.Doc : ""
              })(
                <Input/>)}
            </Item>
            <Item label={"Viết"} {...ITEM_LAYOUT3}>
              {getFieldDecorator('Viet', {
                initialValue: dataEdit.Viet ? dataEdit.Viet : ""
              })(
                <Input/>)}
            </Item>
            <Item label={"Nói"} {...ITEM_LAYOUT3}>
              {getFieldDecorator('Noi', {
                initialValue: dataEdit.Noi ? dataEdit.Noi : ""
              })(
                <Input/>)}
            </Item>
            *Ngoại ngữ theo các mức: A - Yếu, B - Trung bình, C - Khá, D - Thành thạo
          </Form>
        </Modal>
      );
    }
  },
);
export {ModalAddEditNgoaiNgu}