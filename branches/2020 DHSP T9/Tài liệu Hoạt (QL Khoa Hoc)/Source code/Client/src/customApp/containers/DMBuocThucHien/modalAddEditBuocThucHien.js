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
      const checkDangSuDung = dataEdit.DangSuDung != null ? dataEdit.DangSuDung : true;
      this.setState({checkDangSuDung})
    }

    onOk = (e) => {
      e.preventDefault();
      this.props.form.validateFields((err, value) => {
        if (!err) {
          value.TrangThaiSuDung = this.state.checkDangSuDung;
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
          title={actions === 'edit' ? "Sửa thông tin bước thực hiện" : "Thêm thông tin bước thực hiện"}
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
            {getFieldDecorator('BuocThucHienID', {initialValue: dataEdit.BuocThucHienID ? dataEdit.BuocThucHienID : null})}
            <Item label="Tên bước thực hiện" {...ITEM_LAYOUT3}>
              {getFieldDecorator('TenBuocThucHien', {
                initialValue: dataEdit.TenBuocThucHien ? dataEdit.TenBuocThucHien : "",
                rules: [REQUIRED],
              })(<Input autoFocus/>)}
            </Item>
            <Item label="Mã bước thực hiện" {...ITEM_LAYOUT3}>
              {getFieldDecorator('MaBuocThucHien', {
                initialValue: dataEdit.MaBuocThucHien ? dataEdit.MaBuocThucHien : "",
                rules: [REQUIRED],
              })(<Input/>)}
            </Item>
            <Item label="Thứ tự hiển thị" {...ITEM_LAYOUT3}>
              {getFieldDecorator('ThuTuHienThi', {initialValue: dataEdit.ThuTuHienThi ? dataEdit.ThuTuHienThi : 0})
              (<Input onKeyPress={this.inputNumber}/>)}
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