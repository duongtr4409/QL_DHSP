import React, {Component} from 'react';
import {Button, Col, Form, Icon, Input, message, Modal, Row, Upload} from 'antd';
import Constants, {LoaiThongTinNhaKhoaHoc} from "../../../settings/constants";
import {checkValidFileName} from "../../../helpers/utility";

const {ITEM_LAYOUT3} = Constants;
const {Item} = Form;

export const ModalAddEditCacMonGiangDay = Form.create({name: 'modal_add_edit'})(
  class extends Component {
    constructor(props) {
      super(props);
      this.state = {
        FileDinhKem: [],
        DeleteFileDinhKem: []
      }
    }

    onOk = (e) => {
      e.preventDefault();
      this.props.form.validateFields((err, value) => {
        if (!err) {
          const {FileDinhKem} = this.state;
          const {onCreate} = this.props;
          value.DeleteFileDinhKem = this.state.DeleteFileDinhKem;
          if (value.TieuDe === "" && value.DeCuong === "") {
            message.destroy();
            message.warning('Không có thông tin được nhập')
            return;
          }
          onCreate(value, FileDinhKem);
        }
      });
    };

    componentDidMount() {
      const {dataEdit} = this.props;
      const FileDinhKem = dataEdit.FileDinhKem ? dataEdit.FileDinhKem : [];
      this.setState({FileDinhKem})
    }

    onCancel = () => {
      const {onCancel} = this.props;
      onCancel();
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
      const {FileDinhKem} = this.state;
      const filedata = {
        FileUrl: base64,
        FileData: file
      };
      FileDinhKem.push(filedata);
      this.setState({FileDinhKem});
    };

    renderFileDinhKem = () => {
      const {FileDinhKem} = this.state;
      return FileDinhKem.map((item, index) => (
        <Row>
          <Col span={16}>
            <a href={item.FileUrl} download={item.FileDinhKemID ? item.TenFileGoc : item.FileData.name}
               target={"_blank"}>
              {item.FileDinhKemID ? item.TenFileGoc : item.FileData.name}
            </a>
          </Col>
          <Col span={4}/>
          <Col span={4}>
            <Icon onClick={() => this.deleteFileDinhKem(index)} type={'delete'}
                  style={item.FileDinhKemID ? {color: 'red'} : {}}/>
          </Col>
        </Row>
      ))
    };

    deleteFileDinhKem = (index) => {
      const {FileDinhKem, DeleteFileDinhKem} = this.state;
      if (FileDinhKem[index].FileDinhKemID) {
        Modal.confirm({
          title: 'Thông báo',
          content: 'Xác nhận xóa file đính kèm ?',
          okText: 'Có',
          cancelText: 'Không',
          onOk: () => {
            DeleteFileDinhKem.push(FileDinhKem[index].FileDinhKemID)
            FileDinhKem.splice(index, 1);
            this.setState({FileDinhKem, DeleteFileDinhKem});
          }
        });
      } else {
        FileDinhKem.splice(index, 1);
        this.setState({FileDinhKem, DeleteFileDinhKem});
      }
    };

    render() {
      const {visible, onCancel, form, dataEdit, loading, actions} = this.props;
      const {getFieldDecorator} = form;
      let titleModal = "Thêm thông tin môn giảng dạy";
      if (actions === 'edit') {
        titleModal = "Cập nhật thông tin môn giảng dạy";
      }

      return (
        <Modal
          title={titleModal}
          width={600}
          onCancel={this.onCancel}
          visible={visible}
          footer={[
            <Button key="back" onClick={onCancel}>Hủy</Button>,
            <Button key="submit" htmlType="submit" type="primary" form="formgiangday"
                    onClick={this.onOk} loading={loading} disabled={loading}>Lưu</Button>,
          ]}
        >
          <Form id="formgiangday">
            {getFieldDecorator('CTNhaKhoaHocID', {initialValue: dataEdit.CTNhaKhoaHocID ? dataEdit.CTNhaKhoaHocID : null})}
            {getFieldDecorator('CanBoID', {initialValue: this.props.CanBoID})}
            {getFieldDecorator('CoQuanID', {initialValue: this.props.CoQuanID})}
            {getFieldDecorator('LoaiThongTin', {initialValue: LoaiThongTinNhaKhoaHoc.CacMonGiangDay})}
            <Item label={"Tiêu đề"} {...ITEM_LAYOUT3}>
              {getFieldDecorator('TieuDe', {
                initialValue: dataEdit.TieuDe ? dataEdit.TieuDe : ""
              })(<Input autoFocus/>)}
            </Item>
            <Item label={"File đính kèm"} {...ITEM_LAYOUT3}>
              <Upload multiple={true} showUploadList={false}
                      beforeUpload={file => this.beforeUpload(file, this.genDataFile)}>
                <Button>Chọn file</Button>
              </Upload>
            </Item>
            {this.renderFileDinhKem()}
            <Item label={"Đề cương"} {...ITEM_LAYOUT3}>
              {getFieldDecorator('DeCuong', {
                initialValue: dataEdit.DeCuong ? dataEdit.DeCuong : ""
              })(<Input.TextArea autoSize={{minRows: 2}}/>)}
            </Item>
          </Form>
        </Modal>
      );
    }
  },
);