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
      const checkDangSuDung = dataEdit.Status != null ? dataEdit.Status : true;
      this.setState({checkDangSuDung})
    }

    onOk = (e) => {
      e.preventDefault();
      this.props.form.validateFields((err, value) => {
        if (!err) {
          value.Status = this.state.checkDangSuDung;
          // console.log(value);
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
          title={actions === 'edit' ? "Sửa thông tin loại hình nghiên cứu" : "Thêm thông tin loại hình nghiên cứu"}
          width={650}
          visible={visible}
          onCancel={onCancel}
          footer={[
            <Button key="back" onClick={onCancel}>Hủy</Button>,
            <Button key="submit" htmlType="submit" type="primary" form="myForm"
                    onClick={this.onOk} disabled={loading} loading={loading}>Lưu</Button>,
          ]}
        >
          <Form id="myForm" layout="horizontal">
            {getFieldDecorator('Id', {initialValue: dataEdit.Id ? dataEdit.Id : null})}
            <Item label="Tên loại hình nghiên cứu" {...ITEM_LAYOUT3}>
              {getFieldDecorator('Name', {
                initialValue: dataEdit.Name ? dataEdit.Name : "",
                rules: [REQUIRED],
              })(<Input autoFocus/>)}
            </Item>
            <Item label="Ghi chú" {...ITEM_LAYOUT3}>
              {getFieldDecorator('Note', {initialValue: dataEdit.Note ? dataEdit.Note : ""})
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