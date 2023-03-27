import React, {Component} from 'react';
import {Button, Form, Input, Modal, Upload, Checkbox, message, Row, Col, Icon, InputNumber} from 'antd';
import Constants, {LoaiThongTinNhaKhoaHoc} from "../../../settings/constants";
import Select, {Option} from "../../../components/uielements/select";
import TreeSelectEllipsis from "../../components/TreeSelectEllipsis";
import {checkValidFileName} from "../../../helpers/utility";

const {
  ITEM_LAYOUT3, REQUIRED
} = Constants;
const {Item} = Form;

export const ModalAddEditHoatDongKhoaHoc = Form.create({name: 'modal_add_edit'})(
  class extends Component {
    constructor(props) {
      super(props);
      this.state = {
        checkPublic: false,
        FileDinhKem: [],
        DeleteFileDinhKem: []
      };
    }

    onOk = (e) => {
      e.preventDefault();
      this.props.form.validateFields((err, value) => {
        if (!err) {
          const {FileDinhKem} = this.state;
          const {onCreate} = this.props;
          value.DeleteFileDinhKem = this.state.DeleteFileDinhKem;
          value.PublicCV = this.state.checkPublic;
          if (value.NhiemVu == undefined && value.HoatDongKhoaHoc === "") {
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
      const checkPublic = dataEdit.PublicCV != null ? dataEdit.PublicCV : true;
      const FileDinhKem = dataEdit.FileDinhKem ? dataEdit.FileDinhKem : [];
      this.setState({checkPublic, FileDinhKem})
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
      const {visible, onCancel, form, dataEdit, loading, actions, DanhSachNhiemVu} = this.props;
      const {getFieldDecorator} = form;
      let titleModal = "Thêm thông tin hoạt động khoa học";
      if (actions === 'edit') {
        titleModal = "Cập nhật thông tin hoạt động khoa học";
      }

      return (
        <Modal
          title={titleModal}
          width={650}
          onCancel={this.onCancel}
          visible={visible}
          footer={[
            <Button key="back" onClick={onCancel}>Hủy</Button>,
            <Button key="submit" htmlType="submit" type="primary" form="formkhoahoc"
                    onClick={this.onOk} loading={loading} disabled={loading}>Lưu</Button>,
          ]}
        >
          <Form id="formkhoahoc">
            {getFieldDecorator('HoatDongKhoaHocID', {initialValue: dataEdit.HoatDongKhoaHocID ? dataEdit.HoatDongKhoaHocID : null})}
            {getFieldDecorator('CanBoID', {initialValue: this.props.CanBoID})}
            {getFieldDecorator('CoQuanID', {initialValue: this.props.CoQuanID})}
            <Item label={"Nhiệm vụ"} {...ITEM_LAYOUT3}>
              {getFieldDecorator('NhiemVu', {
                initialValue: dataEdit.NhiemVu ? dataEdit.NhiemVu : undefined
              })(
                <TreeSelectEllipsis
                  showSearch
                  data={DanhSachNhiemVu}
                  defaultValue={undefined}
                  dropdownStyle={{maxHeight: 400, overflowX: 'hidden', maxWidth: 500}}
                  placeholder=""
                  allowClear
                  notFoundContent={"Không có dữ liệu"}
                  treeNodeFilterProp={'label'}
                  disabledCategoryId={true}
                />)}
            </Item>
            <Item label={"Hoạt động khoa học"} {...ITEM_LAYOUT3}>
              {getFieldDecorator('HoatDongKhoaHoc', {
                initialValue: dataEdit.HoatDongKhoaHoc ? dataEdit.HoatDongKhoaHoc : ""
              })(
                <Input.TextArea autoSize={{minRows: 2}}/>)}
            </Item>
            <Item label={"Năm thực hiện"} {...ITEM_LAYOUT3}>
              {getFieldDecorator('NamThucHien', {
                initialValue: dataEdit.NamThucHien ? dataEdit.NamThucHien : "", rules: [{...REQUIRED}]
              })(
                <InputNumber min={0}/>)}
            </Item>
            <Item label={"File đính kèm"} {...ITEM_LAYOUT3}>
              <Upload multiple={true} showUploadList={false}
                      beforeUpload={file => this.beforeUpload(file, this.genDataFile)}>
                <Button>Chọn file</Button>
              </Upload>
            </Item>
            {this.renderFileDinhKem()}
            <Item label={"Public lên CV"} {...ITEM_LAYOUT3}>
              <Checkbox checked={this.state.checkPublic}
                        onChange={value => this.setState({checkPublic: value.target.checked})}/>
            </Item>
          </Form>
        </Modal>
      );
    }
  },
);