import React, {Component} from 'react';
import {ITEM_LAYOUT3, REQUIRED} from '../../../settings/constants';
import {Modal, Form, Input, Button, Checkbox} from 'antd';

const {Item} = Form;

const ModalAddEdit = Form.create({name: 'modal_add_edit'})(
  // eslint-disable-next-line
  class extends Component {
    constructor(props) {
      super(props);
      this.state = {
        checkDangSuDung: true
      };
    }

    componentDidMount() {
      const {dataEdit} = this.props;
      const checkDangSuDung = dataEdit.TrangThaiSuDung != null ? dataEdit.TrangThaiSuDung : true;
      this.setState({checkDangSuDung})
    }

    onOk = (e) => {
      e.preventDefault();
      this.props.form.validateFields((err, value) => {
        if (!err) {
          value.TrangThaiSuDung = this.state.checkDangSuDung;
          const {onCreate} = this.props;
          onCreate(value);
        }
      });
    };

    inputNumber = (e) => {
      const key = e.charCode;
      if (key < 48 || key > 57) {
        e.preventDefault();
      }
    };

    render() {
      const {visible, onCancel, form, dataEdit, loading, actions} = this.props;
      const {getFieldDecorator} = form;
      const {checkDangSuDung} = this.state;
      return (
        <Modal
          title={actions === 'edit' ? "Sửa thông tin trạng thái" : "Thêm thông tin trạng thái"}
          width={600}
          visible={visible}
          onCancel={onCancel}
          footer={[
            <Button key="back" onClick={onCancel}>Hủy</Button>,
            <Button key="submit" htmlType="submit" type="primary" form="myForm"
                    onClick={this.onOk} disabled={loading} loading={loading}>Lưu</Button>,
          ]}
        >
          <Form id="myForm" layout="horizontal">
            {getFieldDecorator('TrangThaiID', {initialValue: dataEdit.TrangThaiID ? dataEdit.TrangThaiID : null})}
            <Item label="Tên trạng thái" {...ITEM_LAYOUT3}>
              {getFieldDecorator('TenTrangThai', {
                initialValue: dataEdit.TenTrangThai ? dataEdit.TenTrangThai : "",
                rules: [REQUIRED],
              })(<Input autoFocus/>)}
            </Item>
            <Item label="Ghi chú" {...ITEM_LAYOUT3}>
              {getFieldDecorator('GhiChu', {initialValue: dataEdit.GhiChu ? dataEdit.GhiChu : ""})
              (<Input.TextArea style={{resize: 'none'}} autoSize={{minRows: 3}}/>)}
            </Item>
            <Item label="Đang sử dụng" {...ITEM_LAYOUT3}>
              <Checkbox checked={checkDangSuDung}
                        onChange={value => this.setState({checkDangSuDung: value.target.checked})}/>
            </Item>
          </Form>
        </Modal>
      );
    }
  },
);
export {ModalAddEdit}