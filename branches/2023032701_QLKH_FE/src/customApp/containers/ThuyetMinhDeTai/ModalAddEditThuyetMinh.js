import React, {Component} from "react";
import Constants from "../../../settings/constants";
import {Modal, Form, Upload, Radio, Row, Col, Button, message} from "antd";
import Select, {Option} from "../../../components/uielements/select4";
import {checkValidFileName} from "../../../helpers/utility";

const {Item} = Form;
const {MODAL_NORMAL, ITEM_LAYOUT3, REQUIRED} = Constants;

const ModalAddEdit = Form.create({name: "modal_add_adTemplate"})(
  // eslint-disable-next-line
  class extends Component {
    state = {
      FileDinhKem: null,
      ChuNhiemDeTai: undefined
    };

    componentDidMount() {
      const {actions, dataEdit} = this.props;
      if (actions === 'edit') {
        const {FileThuyetMinh} = dataEdit;
        const ChuNhiemDeTai = `${dataEdit.CanBoID}_${dataEdit.CoQuanID}`;
        const FileDinhKem = {
          FileUrl: FileThuyetMinh.FileUrl,
          FileData: {
            TenFileGoc: FileThuyetMinh.TenFileGoc,
            FileDinhKemID: FileThuyetMinh.FileDinhKemID
          }
        };
        this.setState({ChuNhiemDeTai, FileDinhKem})
      }
    }

    onOk = e => {
      e.preventDefault();
      this.props.form.validateFields((err, value) => {
        if (!err) {
          const {onCreate} = this.props;
          const {ChuNhiemDeTai} = this.state;
          if (ChuNhiemDeTai) {
            const arr = ChuNhiemDeTai.split("_");
            value.CanBoID = arr[0];
            value.CoQuanID = arr[1];
          } else {
            message.destroy();
            message.warn('Chưa chọn chủ nhiệm đề tài');
            return;
          }
          const {FileDinhKem} = this.state;
          if (!FileDinhKem) {
            message.destroy();
            message.warn('Chưa chọn file thuyết minh');
            return;
          }
          onCreate(value, FileDinhKem);
        }
      });
    };

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

    genDataFile = (base64, file) => {
      const filedata = {
        FileUrl: base64,
        FileData: file
      };
      this.setState({FileDinhKem: filedata});
    };

    renderFileDinhKem = () => {
      const {FileDinhKem} = this.state;
      const {actions} = this.props;
      if (FileDinhKem) {
        if (!FileDinhKem.FileData.FileDinhKemID) {
          return <a href={FileDinhKem.FileUrl} target={'_blank'}>{FileDinhKem.FileData.name}</a>
        } else {
          return <a href={FileDinhKem.FileUrl} target={'_blank'}>{FileDinhKem.FileData.TenFileGoc}</a>
        }
      }
    };

    render() {
      const {visible, onCancel, form, loading, dataEdit, actions, DanhSachCanBo} = this.props;
      const {getFieldDecorator} = form;
      const {ChuNhiemDeTai} = this.state;
      return (
        <Modal
          title={actions === 'edit' ? "Sửa thuyết minh đề tài" : "Thêm mới thuyết minh đề tài"}
          okText="Lưu"
          cancelText="Hủy"
          width={MODAL_NORMAL}
          visible={visible}
          onCancel={onCancel}
          footer={[
            <Button key="back" onClick={onCancel}>Hủy</Button>,
            <Button key="submit" htmlType="submit" type="primary" form="myForm"
                    onClick={this.onOk} loading={loading} disabled={loading}>Lưu</Button>,
          ]}
        >
          <Form layout="horizontal">
            {getFieldDecorator('ThuyetMinhID', {initialValue: actions === 'edit' ? dataEdit.ThuyetMinhID : null})}
            <Item label="Thuyết minh" {...ITEM_LAYOUT3}>
              <Upload beforeUpload={file => this.beforeUpload(file, this.genDataFile)} showUploadList={false}>
                <Button type={"primary"}>Chọn file</Button>
                <div style={{marginTop: 20}}>
                  {this.renderFileDinhKem()}
                </div>
              </Upload>
            </Item>
            <Item label="Chủ nhiệm đề tài" {...ITEM_LAYOUT3}>
              <Select value={ChuNhiemDeTai} onChange={value => this.setState({ChuNhiemDeTai: value})} showSearch>
                {DanhSachCanBo.map(item => (
                  <Option value={`${item.CanBoID}_${item.CoQuanID}`}>{item.TenCanBo}</Option>
                ))}
              </Select>
            </Item>
          </Form>
        </Modal>
      );
    }
  }
);
export {ModalAddEdit};
