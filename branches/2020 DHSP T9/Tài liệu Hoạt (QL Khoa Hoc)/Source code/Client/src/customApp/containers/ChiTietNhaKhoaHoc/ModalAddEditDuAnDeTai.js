import React, {Component} from 'react';
import {Button, Form, Input, message, Modal,} from 'antd';
import Constants, {LoaiThongTinNhaKhoaHoc} from "../../../settings/constants";

const {
  ITEM_LAYOUT3,
} = Constants;
const {Item} = Form;

const ModalAddEditDuAnDeTai = Form.create({name: 'modal_add_edit'})(
  class extends Component {
    constructor(props) {
      super(props);
      this.state = {};
    }

    onOk = (e) => {
      e.preventDefault();
      this.props.form.validateFields((err, value) => {
        if (!err) {
          if (value.TenDuAn === "" && value.CoQuanTaiTro === "" && value.KhoangThoiGian === "" && value.VaiTroThamGia === "") {
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
      let titleModal = "Thêm thông tin dự án/ đề tài";
      if (actions === 'edit') {
        titleModal = "Cập nhật thông tin dự án/ đề tài";
      }

      return (
        <Modal
          title={titleModal}
          width={600}
          onCancel={this.onCancel}
          visible={visible}
          footer={[
            <Button key="back" onClick={onCancel}>Hủy</Button>,
            <Button key="submit" htmlType="submit" type="primary" form="formdetai"
                    onClick={this.onOk} loading={loading} disabled={loading}>Lưu</Button>,
          ]}
        >
          <Form id="formdetai">
            {getFieldDecorator('CTNhaKhoaHocID', {initialValue: dataEdit.CTNhaKhoaHocID ? dataEdit.CTNhaKhoaHocID : null})}
            {getFieldDecorator('CanBoID', {initialValue: this.props.CanBoID})}
            {getFieldDecorator('CoQuanID', {initialValue: this.props.CoQuanID})}
            {getFieldDecorator('LoaiThongTin', {initialValue: LoaiThongTinNhaKhoaHoc.DuAnDeTai})}
            <Item label={"Tên dự án/ đề tài"} {...ITEM_LAYOUT3}>
              {getFieldDecorator('TenDuAn', {
                initialValue: dataEdit.TenDuAn ? dataEdit.TenDuAn : ""
              })(
                <Input autoFocus/>)}
            </Item>
            <Item label={"Cơ quan tài trợ"} {...ITEM_LAYOUT3}>
              {getFieldDecorator('CoQuanTaiTro', {
                initialValue: dataEdit.CoQuanTaiTro ? dataEdit.CoQuanTaiTro : ""
              })(
                <Input/>)}
            </Item>
            <Item label={"Thời gian thực hiện"} {...ITEM_LAYOUT3}>
              {getFieldDecorator('KhoangThoiGian', {
                initialValue: dataEdit.KhoangThoiGian ? dataEdit.KhoangThoiGian : ""
              })(
                <Input/>)}
            </Item>
            <Item label={"Vai trò tham gia"} {...ITEM_LAYOUT3}>
              {getFieldDecorator('VaiTroThamGia', {
                initialValue: dataEdit.VaiTroThamGia ? dataEdit.VaiTroThamGia : ""
              })(
                <Input/>)}
            </Item>
          </Form>
        </Modal>
      );
    }
  },
);
export {ModalAddEditDuAnDeTai}