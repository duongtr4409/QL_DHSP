import React, {Component} from 'react';
import {Button, Form, Input, Modal, Row, Col, Upload, message, Icon, InputNumber} from 'antd';
import Constants, {LoaiThongTinNhaKhoaHoc, REQUIRED} from "../../../settings/constants";
import Select, {Option} from "../../../components/uielements/select4";
import {checkValidFileName, formatDataTreeSelect} from "../../../helpers/utility";
import apiConfig from "../DataCoreAPI/config";
import TreeSelectEllipsis from "../../components/TreeSelectEllipsis";

const {
  ITEM_LAYOUT2, ITEM_LAYOUT_HALF2,
  COL_ITEM_LAYOUT_HALF2,
  COL_COL_ITEM_LAYOUT_RIGHT2,
} = Constants;
const {Item} = Form;

const ModalAddEditKetQuaNghienCuu = Form.create({name: 'modal_add_edit'})(
  class extends Component {
    constructor(props) {
      super(props);
      this.state = {
        FileDinhKem: [],
        DeleteFileDinhKem: [],
        ListTacGia: [],
        DanhSachNhiemVu: []
      }
    }

    onOk = (e) => {
      e.preventDefault();
      this.props.form.validateFields((err, value) => {
        if (!err) {
          const {onCreate} = this.props;
          // value.DeleteFileDinhKem = this.state.DeleteFileDinhKem;
          // if (value.DeTai == undefined && value.KhoangThoiGian === "" && value.TacGia === "" && value.TieuDe === "" && value.TenTapChiSachHoiThao === "" && value.So === "" && value.Trang === "" && value.NhaXuatBan === "") {
          //   message.destroy();
          //   message.warning('Không có thông tin được nhập');
          //   return;
          // }
          if (value.TacGia.length > 0) {
            value.ListTacGia = value.TacGia.map(item => {
              const arr = item.split("_");
              return {
                CanBoID: arr[0],
                CoQuanID: arr[1]
              }
            });
            delete value.TacGia;
          }
          onCreate(value);
        }
      });
    };

    componentDidMount() {
      const {dataEdit} = this.props;
      // const FileDinhKem = dataEdit.FileDinhKem ? dataEdit.FileDinhKem : [];
      // this.setState({FileDinhKem})//
      if (dataEdit.ListTacGia) {
        const ListTacGia = dataEdit.ListTacGia.map(item => `${item.CanBoID}_${item.CoQuanID}`);
        this.setState({ListTacGia})
      }
      if (dataEdit.LoaiNhiemVu) {
        this.getDanhSachNhiemVu(dataEdit.LoaiNhiemVu, false);
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

    getDanhSachNhiemVu = (LoaiNhiemVuID, clear = true) => {
      if (clear || !LoaiNhiemVuID) {
        this.props.form.setFieldsValue({NhiemVu: undefined});
      }
      if (LoaiNhiemVuID) {
        apiConfig.DanhSachNhiemVuQuyDoi({categoryId: LoaiNhiemVuID}).then(response => {
          if (response.data.Status > 0) {
            const DanhSachNhiemVu = formatDataTreeSelect(response.data.Data, false);
            this.setState({DanhSachNhiemVu});
          }
        })
      } else {
        this.setState({DanhSachNhiemVu: []})
      }
    };

    render() {
      const {visible, onCancel, form, dataEdit, loading, actions, DanhSachCanBo, DanhSachLoaiNhiemVu, DanhSachDeTai} = this.props;
      const {DanhSachNhiemVu, ListTacGia} = this.state;
      const {getFieldDecorator} = form;
      let titleModal = "Thêm thông tin kết quả nghiên cứu";
      if (actions === 'edit') {
        titleModal = "Cập nhật thông tin kết quả nghiên cứu";
      }

      return (
        <Modal
          title={titleModal}
          width={800}
          onCancel={this.onCancel}
          visible={visible}
          footer={[
            <Button key="back" onClick={onCancel}>Hủy</Button>,
            <Button key="submit" htmlType="submit" type="primary" form="formketqua"
                    onClick={this.onOk} loading={loading} disabled={loading}>Lưu</Button>,
          ]}
        >
          <Form id="formketqua">
            {getFieldDecorator('CTNhaKhoaHocID', {initialValue: dataEdit.CTNhaKhoaHocID ? dataEdit.CTNhaKhoaHocID : null})}
            {getFieldDecorator('CanBoID', {initialValue: this.props.CanBoID})}
            {getFieldDecorator('CoQuanID', {initialValue: this.props.CoQuanID})}
            {getFieldDecorator('LoaiThongTin', {initialValue: LoaiThongTinNhaKhoaHoc.KetQuaNghienCuu})}
            <Item label={"Đề tài"} {...ITEM_LAYOUT2}>
              {getFieldDecorator('DeTai', {
                initialValue: dataEdit.DeTai ? dataEdit.DeTai : undefined
              })(<Select allowClear showSearch autoFocus>
                {DanhSachDeTai.map(item => (
                  <Option value={item.DeTaiID}>{item.TenDeTai}</Option>
                ))}
              </Select>)}
            </Item>
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
            <Item label={"Tên công trình/ văn bằng"} {...ITEM_LAYOUT2}>
              {getFieldDecorator('TieuDe', {
                initialValue: dataEdit.TieuDe ? dataEdit.TieuDe : "", rules: [{...REQUIRED}]
              })(<Input/>)}
            </Item>
            <Row>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Item label={"Thời gian"} {...ITEM_LAYOUT_HALF2}>
                  {getFieldDecorator('NamXuatBan', {
                    initialValue: dataEdit.NamXuatBan ? dataEdit.NamXuatBan : "", rules: [{...REQUIRED}]
                  })(<InputNumber/>)}
                </Item>
              </Col>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Row>
                  <Col {...COL_COL_ITEM_LAYOUT_RIGHT2}>
                    <Item label={"Tác giả"} {...ITEM_LAYOUT_HALF2}>
                      {getFieldDecorator('TacGia', {
                        initialValue: ListTacGia, rules: [{...REQUIRED}]
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
            <Item label={"Ghi chú"} {...ITEM_LAYOUT2}>
              {getFieldDecorator('GhiChu', {
                initialValue: dataEdit.GhiChu ? dataEdit.GhiChu : ""
              })(<Input/>)}
            </Item>
          </Form>
        </Modal>
      );
    }
  },
);
export {ModalAddEditKetQuaNghienCuu}