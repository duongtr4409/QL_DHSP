import React, {Component} from 'react';
import {ITEM_LAYOUT3, REQUIRED} from '../../../settings/constants';
import {Modal, Form, Input, Button, Upload, message, Row, Col, Icon} from 'antd';
import Select, {Option} from '../../../components/uielements/select';
import server from '../../../settings';
import axios from 'axios'
import {checkValidFileName} from "../../../helpers/utility";

const {Item} = Form;
const apiUploadUrl = server.apiUrl + 'AttachFile/UpLoadFile';

const ModalAddEdit = Form.create({name: 'modal_add_edit'})(
  // eslint-disable-next-line
  class extends Component {
    constructor(props) {
      super(props);
      this.state = {
        FileDinhKem: {
          FileUrl: "",
          FileData: null,
          FileDinhKemID: null
        }
      };
    }

    componentDidMount() {
      const {FileDinhKem} = this.props.dataEdit;
      if (FileDinhKem && FileDinhKem.FileDinhKemID) {
        FileDinhKem.FileData = null;
        this.setState({FileDinhKem})
      }
    }

    getBase64 = (file, callback) => {
      const reader = new FileReader();
      reader.addEventListener('load', () => callback(reader.result, file));
      reader.readAsDataURL(file);
    };

    beforeUpload = (file, callback) => {
      const {FileLimit, FileAllow} = this.props;
      const isLt2M = file.size / 1024 / 1024 < FileLimit;
      if (!isLt2M) {
        message.error(`File đính kèm phải nhỏ hơn ${FileLimit}MB`);
      } else if (checkValidFileName(file.name, FileAllow)) {
        message.error('File không hợp lệ');
      } else {
        this.getBase64(file, callback);
      }
      return false;
    };

    onOk = (e) => {
      e.preventDefault();
      this.props.form.validateFields((err, value) => {
        if (!err) {
          const FileDinhKem = this.state.FileDinhKem;
          if (!FileDinhKem.FileData && !FileDinhKem.FileDinhKemID) {
            message.destroy();
            message.warning('Chưa chọn file biểu mẫu đính kèm');
            return;
          }
          const {onCreate} = this.props;
          value.ThuTuHienThi = 0;
          onCreate(value, FileDinhKem);
        }
      });
    };

    inputNumber = (e) => {
      const key = e.charCode;
      if (key < 48 || key > 57) {
        e.preventDefault();
      }
    };

    genFileDinhKem = (base64, file) => {
      const {FileDinhKem} = this.state;
      FileDinhKem.FileUrl = base64;
      FileDinhKem.FileData = file;
      FileDinhKem.FileDinhKemID = null;
      this.setState({FileDinhKem});
    };

    deleteFile = () => {
      const {FileDinhKem} = this.state;
      FileDinhKem.FileData = null;
      FileDinhKem.FileUrl = "";
      FileDinhKem.FileDinhKemID = null;
      this.setState({FileDinhKem});
    };

    render() {
      const {visible, onCancel, form, dataEdit, loading, actions} = this.props;
      const {getFieldDecorator} = form;
      const {FileDinhKem} = this.state;
      return (
        <Modal
          title={actions === 'edit' ? "Sửa thông tin biểu mẫu" : "Thêm thông tin biểu mẫu"}
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
            {getFieldDecorator('BieuMauID', {initialValue: dataEdit.BieuMauID ? dataEdit.BieuMauID : null})}
            <Item label="Tên biểu mẫu" {...ITEM_LAYOUT3}>
              {getFieldDecorator('TenBieuMau', {
                initialValue: dataEdit.TenBieuMau ? dataEdit.TenBieuMau : "",
                rules: [REQUIRED],
              })(<Input autoFocus/>)}
            </Item>
            {/*<Item label="Thứ tự hiển thị" {...ITEM_LAYOUT3}>*/}
            {/*  {getFieldDecorator('ThuTuHienThi', {initialValue: dataEdit.ThuTuHienThi ? dataEdit.ThuTuHienThi : 0})*/}
            {/*  (<Input onKeyPress={this.inputNumber}/>)}*/}
            {/*</Item>*/}
            <Item label="Ghi chú" {...ITEM_LAYOUT3}>
              {getFieldDecorator('GhiChu', {initialValue: dataEdit.GhiChu ? dataEdit.GhiChu : ""})
              (<Input.TextArea style={{resize: 'none'}} autoSize={{minRows: 3}}/>)}
            </Item>
            <Item label="File đính kèm" {...ITEM_LAYOUT3}>
              <Upload showUploadList={false}
                      beforeUpload={file => this.beforeUpload(file, this.genFileDinhKem)}
              >
                <Button style={{marginRight: 15}}>Chọn file</Button>
              </Upload>
              {FileDinhKem.FileUrl !== "" ? FileDinhKem.FileData ?
                <a href={FileDinhKem.FileUrl}
                   download={FileDinhKem.FileData.name}>{FileDinhKem.FileData.name}</a> : !FileDinhKem.FileData && FileDinhKem.FileDinhKemID ?
                  <a href={FileDinhKem.FileUrl}
                     download={FileDinhKem.TenFileGoc}>{FileDinhKem.TenFileGoc}</a> : "" : ""}
            </Item>
          </Form>
        </Modal>
      );
    }
  },
);
export {ModalAddEdit}