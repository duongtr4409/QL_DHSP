import React, {Component} from 'react';
import {Button, Form, Input, Modal, Row, Col, message, Icon} from 'antd';
import Constants, {LoaiThongTinNhaKhoaHoc, REQUIRED} from "../../../settings/constants";
import Select, {Option} from '../../../components/uielements/select4';
import {checkValidFileName} from "../../../helpers/utility";
import TreeSelectEllipsis from "../../components/TreeSelectEllipsis";
import {formatDataTreeSelect} from "../../../helpers/utility";
import apiConfig from '../DataCoreAPI/config';

const {
  ITEM_LAYOUT2, ITEM_LAYOUT_HALF2,
  COL_ITEM_LAYOUT_HALF2,
  COL_COL_ITEM_LAYOUT_RIGHT2,
} = Constants;
const {Item} = Form;

export const ModalAddEditBaoCaoKhoaHoc = Form.create({name: 'modal_add_edit'})(
  class extends Component {
    constructor(props) {
      super(props);
      this.state = {
        FileDinhKem: [],
        DeleteFileDinhKem: [],
        DanhSachNhiemVu: [],
      }
    }

    onOk = (e) => {
      e.preventDefault();
      this.props.form.validateFields((err, value) => {
        if (!err) {
          const {onCreate} = this.props;
          if (value.LoaiNhiemVu == undefined && value.NhiemVu == undefined && value.TieuDe === "" && value.TenHoiThao === "" && value.KhoangThoiGian === "" && value.TacGia.length === 0 && value.ISSN === "") {
            message.destroy();
            message.warning('Không có thông tin được nhập');
            return;
          }
          if (value.TacGia.length > 0) {
            value.ListTacGia = value.TacGia.map(item => {
              const arr = item.split("_");
              return {
                CanBoID: arr[0],
                CoQuanID: arr[1]
              }
            })
          }
          delete value.TacGia;
          onCreate(value);
        }
      });
    };

    componentDidMount() {
      const {dataEdit} = this.props;
      // const FileDinhKem = dataEdit.FileDinhKem ? dataEdit.FileDinhKem : [];
      // this.setState({FileDinhKem})
      if (dataEdit.ListTacGia) {
        dataEdit.ListTacGia = dataEdit.ListTacGia.map(item => `${item.CanBoID}_${item.CoQuanID}`);
      }
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

    getDanhSachNhiemVu = (LoaiNhiemVuID) => {
      this.props.form.setFieldsValue({NhiemVu: undefined});
      apiConfig.DanhSachNhiemVuQuyDoi({categoryId: LoaiNhiemVuID}).then(response => {
        if (response.data.Status > 0) {
          const DanhSachNhiemVu = formatDataTreeSelect(response.data.Data, false);
          this.setState({DanhSachNhiemVu});
        }
      })
    };

    render() {
      const {visible, onCancel, form, dataEdit, loading, actions, DanhSachLoaiNhiemVu, DanhSachCanBo} = this.props;
      const {DanhSachNhiemVu, TacGia} = this.state;
      const {getFieldDecorator} = form;
      let titleModal = "Thêm thông tin bài báo / tạp chí";
      if (actions === 'edit') {
        titleModal = "Cập nhật thông tin bài báo / tạp chí";
      }
      return (
        <Modal
          title={titleModal}
          width={800}
          onCancel={this.onCancel}
          visible={visible}
          footer={[
            <Button key="back" onClick={onCancel}>Hủy</Button>,
            <Button key="submit" htmlType="submit" type="primary" form="formbaocao"
                    onClick={this.onOk} loading={loading} disabled={loading}>Lưu</Button>,
          ]}
        >
          <Form id="formbaocao">
            {getFieldDecorator('CTNhaKhoaHocID', {initialValue: dataEdit.CTNhaKhoaHocID ? dataEdit.CTNhaKhoaHocID : null})}
            {getFieldDecorator('CanBoID', {initialValue: this.props.CanBoID})}
            {getFieldDecorator('CoQuanID', {initialValue: this.props.CoQuanID})}
            {getFieldDecorator('LoaiThongTin', {initialValue: LoaiThongTinNhaKhoaHoc.BaoCaoKhoaHoc})}
            <Row>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Item label={"Loại nhiệm vụ"} {...ITEM_LAYOUT_HALF2}>
                  {getFieldDecorator('LoaiNhiemVu', {
                    initialValue: dataEdit.LoaiNhiemVu ? dataEdit.LoaiNhiemVu : undefined
                  })(
                    <TreeSelectEllipsis
                      showSearch
                      data={DanhSachLoaiNhiemVu}
                      defaultValue={undefined}
                      dropdownStyle={{maxHeight: 400, overflowX: 'hidden', maxWidth: 500}}
                      placeholder=""
                      allowClear
                      notFoundContent={"Không có dữ liệu"}
                      treeNodeFilterProp={'label'}
                      onChange={this.getDanhSachNhiemVu}
                    />
                  )}
                </Item>
              </Col>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Row>
                  <Col {...COL_COL_ITEM_LAYOUT_RIGHT2}>
                    <Item label={"Nhiệm vụ"} {...ITEM_LAYOUT_HALF2}>
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
                        />
                      )}
                    </Item>
                  </Col>
                </Row>
              </Col>
            </Row>
            <Item label={"Tiêu đề"} {...ITEM_LAYOUT2}>
              {getFieldDecorator('TieuDe', {
                initialValue: dataEdit.TieuDe ? dataEdit.TieuDe : ""
              })(<Input/>)}
            </Item>
            <Item label={"Tên hội thảo"} {...ITEM_LAYOUT2}>
              {getFieldDecorator('TenHoiThao', {
                initialValue: dataEdit.TenHoiThao ? dataEdit.TenHoiThao : ""
              })(<Input/>)}
            </Item>
            <Row>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Item label={"Năm xuất bản"} {...ITEM_LAYOUT_HALF2}>
                  {getFieldDecorator('KhoangThoiGian', {
                    initialValue: dataEdit.KhoangThoiGian ? dataEdit.KhoangThoiGian : ""
                  })(<Input/>)}
                </Item>
              </Col>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Row>
                  <Col {...COL_COL_ITEM_LAYOUT_RIGHT2}>
                    <Item label={"Tác giả"} {...ITEM_LAYOUT_HALF2}>
                      {getFieldDecorator('TacGia', {
                        initialValue: dataEdit.ListTacGia ? dataEdit.ListTacGia : []
                      })(
                        <Select showSearch allowClear mode='multiple'>
                          {DanhSachCanBo.map(item => (
                            <Option value={`${item.CanBoID}_${item.CoQuanID}`}>
                              {item.TenCanBo}
                            </Option>
                          ))}
                        </Select>
                      )}
                    </Item>
                  </Col>
                </Row>
              </Col>
            </Row>
            <Item label={"Chỉ số xuất bản (ISSN)"} {...ITEM_LAYOUT2}>
              {getFieldDecorator('ISSN', {
                initialValue: dataEdit.ISSN ? dataEdit.ISSN : ""
              })(<Input/>)}
            </Item>
          </Form>
        </Modal>
      );
    }
  },
);