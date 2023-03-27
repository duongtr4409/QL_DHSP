import React, {Component} from 'react';
import {Button, Col, Form, Icon, Input, message, Modal, Radio, Row, Upload, Checkbox} from 'antd';
import Constants from "../../../settings/constants";
import moment from "moment";
import Select, {Option} from "../../../components/uielements/select";
import DatePicker from "../../../components/uielements/datePickerFormat";
import ImgCrop from "antd-img-crop";
import {checkValidFileName} from "../../../helpers/utility";

const {
  ITEM_LAYOUT2,
  ITEM_LAYOUT_HALF2,
  COL_ITEM_LAYOUT_HALF2,
  COL_COL_ITEM_LAYOUT_RIGHT2,
  REQUIRED,
} = Constants;
const {Item} = Form;
const {Group} = Radio;

const ModalAddEdit = Form.create({name: 'modal_add_edit_taikhoan'})(
  class extends Component {
    constructor(props) {
      super(props);
      this.state = {
        loadingAvatar: false,
        AnhDaiDien: {
          FileUrl: "",
          FileData: null,
          FileDinhKemID: null
        },
        checkChuyenGia: false,
        FileLyLich: []
      };
    }

    getBase64 = (file, callback) => {
      const reader = new FileReader();
      reader.addEventListener('load', () => callback(reader.result, file));
      reader.readAsDataURL(file);
    };

    beforeUpload = (file, callback) => {
      const {FileLimit} = this.props;
      const isJpgOrPng = file.type === 'image/jpeg' || file.type === 'image/png';
      const isLt2M = file.size / 1024 / 1024 < FileLimit;
      if (!isJpgOrPng) {
        message.error('Sai định dạng ảnh (JPG hoặc PNG)');
      } else if (!isLt2M) {
        message.error(`File ảnh phải nhỏ hơn ${FileLimit}MB`);
      } else {
        const _URL = window.URL || window.webkitURL;
        const img = new Image();
        const objectUrl = _URL.createObjectURL(file);
        img.onload = () => {
          const height = img.height;
          const width = img.width;
          // console.log(width, height);
          if (height > width * 3 || width > height * 3) {
            message.error(`Kích cỡ ảnh không phù hợp để làm ảnh hồ sơ`);
          } else {
            this.getBase64(file, callback);
          }
          _URL.revokeObjectURL(objectUrl);
        };
        img.src = objectUrl;
      }
      return false;
    };

    beforeUploadFile = (file, callback) => {
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

    upperFirstLetter = (word) => {
      let text = word.split(' ');
      let res = [];
      for (let i = 0; i < text.length; i++) {
        let text2 = text[i].split('');
        text2[0] = text2[0].toUpperCase();
        text2 = text2.join('');
        res[res.length] = text2;
      }
      return res.join(' ');
    };

    onOk = (e) => {
      e.preventDefault();
      this.props.form.validateFields((err, value) => {
        if (!err) {
          const {checkChuyenGia} = this.state;
          value.LaChuyenGia = !!checkChuyenGia;
          value.TenCanBo = this.upperFirstLetter(value.TenCanBo);
          // console.log(value);
          const {onCreate} = this.props;
          onCreate(value, this.state.AnhDaiDien, this.state.FileLyLich);
        }
      });
    };

    disabledDate(current) {
      // Can not select days after today and today
      return current && current > moment().endOf("day");
    }

    inputTen = (e) => {
      const key = e.charCode;
      if (
        (key === 32 && e.target.value[e.target.value.length - 1] === " ") ||
        (key === 32 && e.target.value.length === 0)
      ) {
        e.preventDefault();
      }
    };

    inputNumber = (e) => {
      const key = e.charCode;
      if (key < 48 || key > 57) {
        e.preventDefault();
      }
    };

    onCancel = () => {
      const {onCancel} = this.props;
      onCancel();
    };

    changeCheckChuyenGia = (value) => {
      this.setState({checkChuyenGia: value.target.checked});
    };

    genAnhDaiDien = (base64, file) => {
      const {AnhDaiDien} = this.state;
      AnhDaiDien.FileUrl = base64;
      AnhDaiDien.FileData = file;
      this.setState({AnhDaiDien});
    };

    genFileDinhKem = (base64, file) => {
      const {FileLyLich} = this.state;
      const filedinhkem = {
        FileUrl: base64,
        FileData: file
      };
      FileLyLich.push(filedinhkem);
      this.setState({FileLyLich});
    };

    renderFileLyLich = (FileLyLich) => {
      if (FileLyLich && FileLyLich.length > 0) {
        return FileLyLich.map((file, index) => (
          <Row>
            <Col span={16}>
              <a href={file.FileUrl}>
                {file.FileDinhKemID ? file.TenFileGoc : file.FileData.name}
              </a>
            </Col>
            <Col span={4}/>
            <Col span={4} style={{textAlign: 'center'}}>
              <Icon type={'delete'} onClick={() => this.deleteFileDinhKem(index)}/>
            </Col>
          </Row>
        ))
      }
    };

    deleteFileDinhKem = (index) => {
      const {FileLyLich} = this.state;
      FileLyLich.splice(index, 1);
      this.setState({FileLyLich})
    };

    checkFileImage = (file) => {
      const {FileLimit} = this.props;
      const isJpgOrPng = file.type === 'image/jpeg' || file.type === 'image/png';
      const isLt2M = file.size / 1024 / 1024 < FileLimit;
      if (!isJpgOrPng) {
        message.error('Sai định dạng ảnh (JPG hoặc PNG)');
      }
      if (!isLt2M) {
        message.error(`File ảnh phải nhỏ hơn ${FileLimit}MB`);
      }
      return isJpgOrPng && isLt2M;
    };

    render() {
      const {visible, onCancel, form, loading, DanhSachChucDanh, DanhSachHocVi} = this.props;
      const {checkChuyenGia, FileLyLich} = this.state;
      const {AnhDaiDien} = this.state;
      const {getFieldDecorator} = form;

      const uploadButton = (
        <div>
          <Icon type={this.state.loading ? 'loading' : 'plus'}/>
          <div className="ant-upload-text">Tải ảnh lên</div>
        </div>
      );

      return (
        <Modal
          title={"Thêm thông tin nhà khoa học"}
          width={1000}
          onCancel={this.onCancel}
          visible={visible}
          footer={[
            <Button key="back" onClick={onCancel}>Hủy</Button>,
            <Button key="submit" htmlType="submit" type="primary" form="myForm"
                    onClick={this.onOk} loading={loading} disabled={loading}>Lưu</Button>,
          ]}
        >
          <Form id="form-nha-khoa-hoc">
            <Item label="Ảnh đại diện" {...ITEM_LAYOUT2}>
              <ImgCrop grid rotate modalOk={"Cắt ảnh"} modalCancel={"Hủy"} modalTitle={"Chỉnh sửa hình ảnh"}
                       maxZoom={5} beforeCrop={this.checkFileImage} shape={'round'}>
                <Upload
                  name="avatar"
                  listType="picture-card"
                  className="avatar-uploader"
                  showUploadList={false}
                  beforeUpload={file => this.beforeUpload(file, this.genAnhDaiDien)}
                  accept={".png, .jpg, .jpeg"}
                >
                  {AnhDaiDien.FileUrl !== "" ?
                    <img src={AnhDaiDien.FileUrl} alt="avatar" style={{width: '100%'}} id={'avatar'}/> : uploadButton}
                </Upload>
              </ImgCrop>
            </Item>
            <Row>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Item label={"Mã cán bộ"} {...ITEM_LAYOUT_HALF2}>
                  {getFieldDecorator('MaCB', {
                    initialValue: ""
                  })(
                    <Input autoFocus/>)}
                </Item>
              </Col>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Row>
                  <Col {...COL_COL_ITEM_LAYOUT_RIGHT2}>
                    <Item label={"Tên cán bộ"} {...ITEM_LAYOUT_HALF2}>
                      {getFieldDecorator('TenCanBo', {
                        initialValue: "",
                        rules: [{...REQUIRED}]
                      })
                      (<Input onKeyPress={this.inputTen} style={{textTransform: "capitalize"}}/>)
                      }
                    </Item>
                  </Col>
                </Row>
              </Col>
            </Row>
            <Row>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Item label={"Ngày sinh"} {...ITEM_LAYOUT_HALF2} className={'datepicker'}>
                  {getFieldDecorator('NgaySinh', {
                    initialValue: ""
                  })
                  (
                    <DatePicker format={'DD/MM/YYYY'} placeholder={""} style={{width: "100%"}}
                                disabledDate={this.disabledDate}/>
                  )}
                </Item>
              </Col>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Row>
                  <Col {...COL_COL_ITEM_LAYOUT_RIGHT2}>
                    <Item label={"Giới tính"} {...ITEM_LAYOUT_HALF2}>
                      {getFieldDecorator('GioiTinh', {
                        initialValue: 1,
                      })(
                        <Group>
                          <Radio value={1}>Nam</Radio>
                          <Radio value={0}>Nữ</Radio>
                        </Group>,
                      )}
                    </Item>
                  </Col>
                </Row>
              </Col>
            </Row>
            <Row>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Item label={"Học hàm, học vị"} {...ITEM_LAYOUT_HALF2}>
                  {getFieldDecorator('ChucDanhKhoaHoc', {
                    initialValue: undefined
                  })(
                    <Select style={{width: '100%'}} allowClear showSearch mode={"multiple"}>
                      {DanhSachHocVi.map(item => (
                        <Option value={item.Id}>{item.Name}</Option>
                      ))}
                    </Select>)}
                </Item>
              </Col>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Row>
                  <Col {...COL_COL_ITEM_LAYOUT_RIGHT2}>
                    <Item label={"Chức danh hành chính"} {...ITEM_LAYOUT_HALF2}>
                      {getFieldDecorator('ChucDanhHanhChinh', {
                        initialValue: undefined,
                      })(
                        <Select style={{width: '100%'}} allowClear showSearch mode={"multiple"}>
                          {DanhSachChucDanh.map(item => (
                            <Option value={item.Id}>{item.Name}</Option>
                          ))}
                        </Select>)}
                    </Item>
                  </Col>
                </Row>
              </Col>
            </Row>
            <Row>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Item label={"Cơ quan công tác"} {...ITEM_LAYOUT_HALF2}>
                  {getFieldDecorator('CoQuanCongTac', {
                    initialValue: ""
                  })(
                    <Input/>)}
                </Item>
              </Col>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Row>
                  <Col {...COL_COL_ITEM_LAYOUT_RIGHT2}>
                    <Item label={"Địa chỉ cơ quan"} {...ITEM_LAYOUT_HALF2}>
                      {getFieldDecorator('DiaChiCoQuan', {
                        initialValue: "",
                      })(
                        <Input/>)}
                    </Item>
                  </Col>
                </Row>
              </Col>
            </Row>
            <Row>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Item label={"Điện thoại"} {...ITEM_LAYOUT_HALF2}>
                  {getFieldDecorator('DienThoai', {
                    initialValue: ""
                  })
                  (<Input onKeyPress={this.inputNumber}/>)}
                </Item>
              </Col>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Row>
                  <Col {...COL_COL_ITEM_LAYOUT_RIGHT2}>
                    <Item label={"Email"} {...ITEM_LAYOUT_HALF2}>
                      {getFieldDecorator('Email', {
                        initialValue: "",
                        rules: [{type: 'email', message: 'Email không hợp lệ'}, REQUIRED]
                      })(<Input/>)}
                    </Item>
                  </Col>
                </Row>
              </Col>
            </Row>
            <Row>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Item label={"Fax"} {...ITEM_LAYOUT_HALF2}>
                  {getFieldDecorator('Fax', {
                    initialValue: ""
                  })
                  (<Input onKeyPress={this.inputNumber}/>)}
                </Item>
              </Col>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Row>
                  <Col {...COL_COL_ITEM_LAYOUT_RIGHT2}>
                    <Item allowClear={true} label={"Điện thoại di động"} {...ITEM_LAYOUT_HALF2}>
                      {getFieldDecorator('DienThoaiDiDong', {
                        initialValue: ""
                      })
                      (<Input onKeyPress={this.inputNumber}/>)}
                    </Item>
                  </Col>
                </Row>
              </Col>
            </Row>
            <Row>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Item label={"Chuyên gia"} {...ITEM_LAYOUT_HALF2}>
                  <Checkbox checked={checkChuyenGia} onChange={this.changeCheckChuyenGia}/>
                </Item>
              </Col>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Row>
                  <Col {...COL_COL_ITEM_LAYOUT_RIGHT2}>
                    <Item allowClear={true} label={"File lý lịch"} {...ITEM_LAYOUT_HALF2}
                          style={{lineHeight: 30}}>
                      <Upload showUploadList={false} multiple={true}
                              beforeUpload={file => this.beforeUploadFile(file, this.genFileDinhKem)}>
                        <Button style={{width: 100, marginRight: 10}}>Chọn file</Button>
                      </Upload>
                      {this.renderFileLyLich(FileLyLich)}
                    </Item>
                  </Col>
                </Row>
              </Col>
            </Row>
          </Form>
        </Modal>
      );
    }
  },
);
export {ModalAddEdit}