import React, { Component } from "react";
import { Button, Col, Form, Icon, Input, message, Modal, Row, InputNumber } from "antd";
import Constants, { LoaiThongTinNhaKhoaHoc, REQUIRED } from "../../../settings/constants";
import Select, { Option } from "../../../components/uielements/select4";
import { checkValidFileName, formatDataTreeSelect } from "../../../helpers/utility";
import TreeSelectEllipsis from "../../components/TreeSelectEllipsis";
import apiConfig from "../DataCoreAPI/config";
import Datepicker from "../../../components/uielements/datePickerFormat";
import moment from "moment";

const { ITEM_LAYOUT2, ITEM_LAYOUT_HALF2, COL_ITEM_LAYOUT_HALF2, COL_COL_ITEM_LAYOUT_RIGHT2 } = Constants;
const { Item } = Form;

export const ModalAddEditBaiBaoTapChi = Form.create({ name: "modal_add_edit" })(
  class extends Component {
    constructor(props) {
      super(props);
      this.state = {
        FileDinhKem: [],
        DeleteFileDinhKem: [],
        DanhSachNhiemVu: [],
        ListTacGia: [],
        LoaiBaiBao: 0,
      };
    }

    onOk = (e) => {
      e.preventDefault();
      this.props.form.validateFields((err, value) => {
        if (!err) {
          const { onCreate } = this.props;
          if (value.TacGia.length > 0) {
            value.ListTacGia = value.TacGia.map((item) => {
              const arr = item.split("_");
              return {
                CanBoID: arr[0],
                CoQuanID: arr[1],
              };
            });
          }
          delete value.TacGia;
          if (value.NgayHoiThao) {
            value.NgayHoiThao = moment(value.NgayHoiThao).format("yyyy-MM-DD");
          }
          if (value.LinkBaiBao !== "" && value.LinkBaiBao != null) {
            const isvalidLink = this.validUrl(value.LinkBaiBao);
            if (!isvalidLink) {
              message.destroy();
              message.warn("Link bài báo không hợp lệ");
              return;
            }
          }
          onCreate(value);
        }
      });
    };

    componentDidMount() {
      const { dataEdit } = this.props;
      if (dataEdit.ListTacGia) {
        const ListTacGia = dataEdit.ListTacGia.map((item) => `${item.CanBoID}_${item.CoQuanID}`);
        this.setState({ ListTacGia });
      }
      if (dataEdit.LoaiNhiemVu) {
        this.getDanhSachNhiemVu(dataEdit.LoaiNhiemVu, false);
      }
      if (dataEdit.LoaiBaiBao) {
        this.setState({ LoaiBaiBao: dataEdit.LoaiBaiBao });
      }
    }

    onCancel = () => {
      const { onCancel } = this.props;
      onCancel();
    };

    getBase64 = (file, callback) => {
      const reader = new FileReader();
      reader.addEventListener("load", () => callback(reader.result, file));
      reader.readAsDataURL(file);
    };

    beforeUpload = (file, callback) => {
      const { FileLimit, FileAllow } = this.props;
      const isLt2M = file.size / 1024 / 1024 < FileLimit;
      if (!isLt2M) {
        message.error(`File đính kèm phải nhỏ hơn ${FileLimit}MB`);
      } else if (checkValidFileName(file.name, FileAllow)) {
        message.error("File không hợp lệ");
      } else {
        this.getBase64(file, callback);
      }
      return false;
    };

    genDataFile = (base64, file) => {
      const { FileDinhKem } = this.state;
      const filedata = {
        FileUrl: base64,
        FileData: file,
      };
      FileDinhKem.push(filedata);
      this.setState({ FileDinhKem });
    };

    renderFileDinhKem = () => {
      const { FileDinhKem } = this.state;
      return FileDinhKem.map((item, index) => (
        <Row>
          <Col span={16}>
            <a href={item.FileUrl} download={item.FileDinhKemID ? item.TenFileGoc : item.FileData.name} target={"_blank"}>
              {item.FileDinhKemID ? item.TenFileGoc : item.FileData.name}
            </a>
          </Col>
          <Col span={4} />
          <Col span={4}>
            <Icon onClick={() => this.deleteFileDinhKem(index)} type={"delete"} style={item.FileDinhKemID ? { color: "red" } : {}} />
          </Col>
        </Row>
      ));
    };

    deleteFileDinhKem = (index) => {
      const { FileDinhKem, DeleteFileDinhKem } = this.state;
      if (FileDinhKem[index].FileDinhKemID) {
        Modal.confirm({
          title: "Thông báo",
          content: "Xác nhận xóa file đính kèm ?",
          okText: "Có",
          cancelText: "Không",
          onOk: () => {
            DeleteFileDinhKem.push(FileDinhKem[index].FileDinhKemID);
            FileDinhKem.splice(index, 1);
            this.setState({ FileDinhKem, DeleteFileDinhKem });
          },
        });
      } else {
        FileDinhKem.splice(index, 1);
        this.setState({ FileDinhKem, DeleteFileDinhKem });
      }
    };

    getDanhSachNhiemVu = (LoaiNhiemVuID, clear = true) => {
      if (clear || !LoaiNhiemVuID) {
        this.props.form.setFieldsValue({ NhiemVu: undefined });
      }
      if (LoaiNhiemVuID) {
        apiConfig.DanhSachNhiemVuQuyDoi({ categoryId: LoaiNhiemVuID }).then((response) => {
          if (response.data.Status > 0) {
            const DanhSachNhiemVu = formatDataTreeSelect(response.data.Data, false);
            this.setState({ DanhSachNhiemVu });
          }
        });
      } else {
        this.setState({ DanhSachNhiemVu: [] });
      }
    };

    clickUrl = (value) => {
      const url = value.target.value;
      if (this.validUrl(url)) {
        window.open(url);
      }
    };

    validUrl = (url) => {
      if (!url) {
        url = this.props.form.getFieldValue("LinkBaiBao");
      }
      const regexUrl = /https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,4}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)/gm;
      return regexUrl.test(url);
    };

    render() {
      const { visible, onCancel, form, dataEdit, loading, actions, DanhSachLoaiNhiemVu, DanhSachCanBo, DanhSachDeTai, DanhSachPhongBan } = this.props;
      const { DanhSachNhiemVu, ListTacGia, LoaiBaiBao } = this.state;
      const { getFieldDecorator } = form;
      let titleModal = "Thêm thông tin bài báo / tạp chí";
      if (actions === "edit") {
        titleModal = "Cập nhật thông tin bài báo / tạp chí";
      }
      return (
        <Modal
          title={titleModal}
          width={1000}
          onCancel={this.onCancel}
          visible={visible}
          footer={[
            <Button key="back" onClick={onCancel}>
              Hủy
            </Button>,
            <Button key="submit" htmlType="submit" type="primary" form="formtapchi" onClick={this.onOk} loading={loading} disabled={loading}>
              Lưu
            </Button>,
          ]}
        >
          <Form id="formtapchi">
            {getFieldDecorator("CTNhaKhoaHocID", { initialValue: dataEdit.CTNhaKhoaHocID ? dataEdit.CTNhaKhoaHocID : null })}
            {getFieldDecorator("CanBoID", { initialValue: this.props.CanBoID })}
            {getFieldDecorator("CoQuanID", { initialValue: this.props.CoQuanID })}
            {getFieldDecorator("LoaiThongTin", { initialValue: LoaiThongTinNhaKhoaHoc.BaiBaoTapChi })}
            <Row>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Item label={"Phân loại"} {...ITEM_LAYOUT_HALF2}>
                  {getFieldDecorator("LoaiBaiBao", {
                    initialValue: dataEdit.LoaiBaiBao ? dataEdit.LoaiBaiBao : undefined,
                    rules: [{ ...REQUIRED }],
                  })(
                    <Select autoFocus showSearch allowClear onChange={(value) => this.setState({ LoaiBaiBao: value })}>
                      <Option value={1}>Bài báo quốc tế</Option>
                      <Option value={2}>Bài báo trong nước</Option>
                      <Option value={3}>Hội thảo quốc tế</Option>
                      <Option value={4}>Hội thảo trong nước</Option>
                    </Select>
                  )}
                </Item>
              </Col>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Row>
                  <Col {...COL_COL_ITEM_LAYOUT_RIGHT2}>
                    <Item label={"Đề tài"} {...ITEM_LAYOUT_HALF2}>
                      {getFieldDecorator("DeTai", {
                        initialValue: dataEdit.DeTai ? dataEdit.DeTai : undefined,
                      })(
                        <Select showSearch>
                          {DanhSachDeTai.map((item) => (
                            <Option value={item.DeTaiID}>{item.TenDeTai}</Option>
                          ))}
                        </Select>
                      )}
                    </Item>
                  </Col>
                </Row>
              </Col>
            </Row>
            <Row>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Item label={"Loại nhiệm vụ"} {...ITEM_LAYOUT_HALF2}>
                  {getFieldDecorator("LoaiNhiemVu", {
                    initialValue: dataEdit.LoaiNhiemVu ? dataEdit.LoaiNhiemVu : undefined,
                  })(
                    <TreeSelectEllipsis
                      showSearch
                      data={DanhSachLoaiNhiemVu}
                      defaultValue={undefined}
                      dropdownStyle={{ maxHeight: 400, overflowX: "hidden", maxWidth: 500 }}
                      placeholder=""
                      allowClear
                      notFoundContent={"Không có dữ liệu"}
                      treeNodeFilterProp={"label"}
                      onChange={this.getDanhSachNhiemVu}
                    />
                  )}
                </Item>
              </Col>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Row>
                  <Col {...COL_COL_ITEM_LAYOUT_RIGHT2}>
                    <Item label={"Nhiệm vụ"} {...ITEM_LAYOUT_HALF2}>
                      {getFieldDecorator("NhiemVu", {
                        initialValue: dataEdit.NhiemVu ? dataEdit.NhiemVu : undefined,
                      })(
                        <TreeSelectEllipsis
                          showSearch
                          data={DanhSachNhiemVu}
                          defaultValue={undefined}
                          dropdownStyle={{ maxHeight: 400, overflowX: "hidden", maxWidth: 500 }}
                          placeholder=""
                          allowClear
                          notFoundContent={"Không có dữ liệu"}
                          treeNodeFilterProp={"label"}
                        />
                      )}
                    </Item>
                  </Col>
                </Row>
              </Col>
            </Row>
            <Item label={"Tên bài báo"} {...ITEM_LAYOUT2}>
              {getFieldDecorator("TieuDe", {
                initialValue: dataEdit.TieuDe ? dataEdit.TieuDe : "",
                rules: [{ ...REQUIRED }],
              })(<Input />)}
            </Item>
            <Item label={"Tác giả"} {...ITEM_LAYOUT2}>
              {getFieldDecorator("TacGia", {
                initialValue: ListTacGia,
                rules: [{ ...REQUIRED }],
              })(
                <Select showSearch allowClear mode="multiple">
                  {DanhSachCanBo.map((item) => (
                    <Option value={`${item.CanBoID}_${item.CoQuanID}`}>{item.TenCanBo}</Option>
                  ))}
                </Select>
              )}
            </Item>
            <Item label={`Tên ${LoaiBaiBao >= 3 ? "hội thảo" : "tạp chí"}`} {...ITEM_LAYOUT2}>
              {getFieldDecorator("TenTapChiSachHoiThao", {
                initialValue: dataEdit.TenTapChiSachHoiThao ? dataEdit.TenTapChiSachHoiThao : "",
                rules: [{ ...REQUIRED }],
              })(<Input />)}
            </Item>
            {LoaiBaiBao === 1
              ? [
                  <Row>
                    <Col {...COL_ITEM_LAYOUT_HALF2}>
                      <Item label={"Hệ số ảnh hường (IF)"} {...ITEM_LAYOUT_HALF2}>
                        {getFieldDecorator("HeSoAnhHuong", {
                          initialValue: dataEdit.HeSoAnhHuong ? dataEdit.HeSoAnhHuong : "",
                        })(<Input />)}
                      </Item>
                    </Col>
                    <Col {...COL_ITEM_LAYOUT_HALF2}>
                      <Row>
                        <Col {...COL_COL_ITEM_LAYOUT_RIGHT2}>
                          <Item label={"Chỉ số"} {...ITEM_LAYOUT_HALF2}>
                            {getFieldDecorator("ChiSo", {
                              initialValue: dataEdit.ChiSo ? dataEdit.ChiSo : undefined,
                            })(
                              <Select allowClear showSearch>
                                <Option value={1}>ISI</Option>
                                <Option value={2}>SCOPUS</Option>
                                <Option value={3}>Khác</Option>
                              </Select>
                            )}
                          </Item>
                        </Col>
                      </Row>
                    </Col>
                  </Row>,
                  <Row>
                    <Col {...COL_ITEM_LAYOUT_HALF2}>
                      <Item label={"Rank theo SCIMAG"} {...ITEM_LAYOUT_HALF2}>
                        {getFieldDecorator("RankSCIMAG", {
                          initialValue: dataEdit.RankSCIMAG ? dataEdit.RankSCIMAG : undefined,
                        })(
                          <Select allowClear showSearch>
                            <Option value={1}>Q1</Option>
                            <Option value={2}>Q2</Option>
                            <Option value={3}>Q3</Option>
                            <Option value={4}>Q4</Option>
                          </Select>
                        )}
                      </Item>
                    </Col>
                  </Row>,
                ]
              : ""}
            {LoaiBaiBao === 2 ? (
              <Row>
                <Col {...COL_ITEM_LAYOUT_HALF2}>
                  <Item label={"Điểm tạp chí"} {...ITEM_LAYOUT_HALF2}>
                    {getFieldDecorator("DiemTapChi", {
                      initialValue: dataEdit.DiemTapChi ? dataEdit.DiemTapChi : undefined,
                    })(
                      <Select>
                        <Option value={1}>0</Option>
                        <Option value={2}>0.25</Option>
                        <Option value={3}>0.5</Option>
                        <Option value={4}>0.75</Option>
                        <Option value={5}>1</Option>
                      </Select>
                    )}
                  </Item>
                </Col>
              </Row>
            ) : (
              ""
            )}
            {LoaiBaiBao > 2 ? (
              <Row>
                <Col {...COL_ITEM_LAYOUT_HALF2}>
                  <Item label={"Cấp hội thảo"} {...ITEM_LAYOUT_HALF2}>
                    {getFieldDecorator("CapHoiThao", {
                      initialValue: dataEdit.CapHoiThao ? dataEdit.CapHoiThao : undefined,
                    })(
                      <Select>
                        <Option value={1}>Quốc tế</Option>
                        <Option value={2}>Quốc gia</Option>
                        <Option value={3}>Trong nước</Option>
                      </Select>
                    )}
                  </Item>
                </Col>
              </Row>
            ) : (
              ""
            )}
            <Row>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Item label={`Số ${LoaiBaiBao > 2 ? "ISBN" : "ISSN"}`} {...ITEM_LAYOUT_HALF2}>
                  {getFieldDecorator("ISSN", {
                    initialValue: dataEdit.ISSN ? dataEdit.ISSN : "",
                  })(<Input />)}
                </Item>
              </Col>
            </Row>
            <Row>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Item label={"Tập"} {...ITEM_LAYOUT_HALF2}>
                  {getFieldDecorator("Tap", {
                    initialValue: dataEdit.Tap ? dataEdit.Tap : "",
                  })(<Input />)}
                </Item>
              </Col>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Row>
                  <Col {...COL_COL_ITEM_LAYOUT_RIGHT2}>
                    <Item label={"Số"} {...ITEM_LAYOUT_HALF2}>
                      {getFieldDecorator("So", {
                        initialValue: dataEdit.So ? dataEdit.So : "",
                      })(<Input />)}
                    </Item>
                  </Col>
                </Row>
              </Col>
            </Row>
            <Row>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Item label={"Năm đăng tải"} {...ITEM_LAYOUT_HALF2}>
                  {getFieldDecorator("NamDangTai", {
                    initialValue: dataEdit.NamDangTai ? dataEdit.NamDangTai : "",
                    rules: [{ ...REQUIRED }],
                  })(<InputNumber min={0} />)}
                </Item>
              </Col>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Row>
                  <Col {...COL_COL_ITEM_LAYOUT_RIGHT2}>
                    <Item label={"Từ trang đến trang"} {...ITEM_LAYOUT_HALF2}>
                      {getFieldDecorator("Trang", {
                        initialValue: dataEdit.Trang ? dataEdit.Trang : "",
                      })(<Input />)}
                    </Item>
                  </Col>
                </Row>
              </Col>
            </Row>
            <Row>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Item label={"Link bài báo"} {...ITEM_LAYOUT_HALF2}>
                  {getFieldDecorator("LinkBaiBao", {
                    initialValue: dataEdit.LinkBaiBao ? dataEdit.LinkBaiBao : "",
                  })(<Input onDoubleClick={(value) => this.clickUrl(value)} className={`${this.validUrl() ? "input-url" : ""}`} />)}
                </Item>
              </Col>
              <Col {...COL_ITEM_LAYOUT_HALF2}>
                <Row>
                  <Col {...COL_COL_ITEM_LAYOUT_RIGHT2}>
                    <Item label={"Lĩnh vực/ Ngành khoa học"} {...ITEM_LAYOUT_HALF2}>
                      {getFieldDecorator("LinhVucNganhKhoaHoc", {
                        initialValue: dataEdit.LinhVucNganhKhoaHoc ? dataEdit.LinhVucNganhKhoaHoc : undefined,
                      })(
                        <Select allowClear showSearch>
                          {DanhSachPhongBan.map((item) => (
                            <Option value={item.Id}>{item.Name}</Option>
                          ))}
                        </Select>
                      )}
                    </Item>
                  </Col>
                </Row>
              </Col>
            </Row>
            {LoaiBaiBao > 2 ? (
              <Row>
                <Col {...COL_ITEM_LAYOUT_HALF2}>
                  <Item label={"Ngày hội thảo"} {...ITEM_LAYOUT_HALF2}>
                    {getFieldDecorator("NgayHoiThao", {
                      initialValue: dataEdit.NgayHoiThao ? moment(dataEdit.NgayHoiThao) : "",
                    })(<Datepicker format={"DD/MM/YYYY"} placeholder={""} />)}
                  </Item>
                </Col>
                <Col {...COL_ITEM_LAYOUT_HALF2}>
                  <Row>
                    <Col {...COL_COL_ITEM_LAYOUT_RIGHT2}>
                      <Item label={"Địa điểm tổ chức"} {...ITEM_LAYOUT_HALF2}>
                        {getFieldDecorator("DiaDiemToChuc", {
                          initialValue: dataEdit.DiaDiemToChuc ? dataEdit.DiaDiemToChuc : "",
                        })(<Input />)}
                      </Item>
                    </Col>
                  </Row>
                </Col>
              </Row>
            ) : (
              ""
            )}
          </Form>
        </Modal>
      );
    }
  }
);
