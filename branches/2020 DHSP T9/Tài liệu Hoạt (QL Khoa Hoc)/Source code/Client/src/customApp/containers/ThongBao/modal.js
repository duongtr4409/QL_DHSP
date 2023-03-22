import React from "react";
import { Modal, Icon, Button, Input, Upload, message, DatePicker, TreeSelect, Tooltip, Checkbox, Collapse, Table, Tag } from "antd";
import { DatePicker as DatePicker4 } from "antd4";
import { GoDatePicker, GoInput, TreeSelectWithApi } from "../../components/index";

import moment from "moment";
import api from "./config";
import { ValidatorForm } from "react-form-validator-core";
import lodash from "lodash";
import { checkFilesSize, checkFileType } from "../../../helpers/utility";
import { find, walk, getFlatDataFromTree, getTreeFromFlatData } from "../../../helpers/tree-helper";
import BoxTable, { EmptyTable } from "../../../components/utility/boxTable";

const fields = [
  { key: "TenThongBao", name: "Tên thông báo" },
  { key: "NoiDung", name: "Nội dung" },
  { key: "ThoiGianBatDau", name: "Thời gian bắt đầu" },
  { key: "ThoiGianKetThuc", name: "Thời gian kết thúc" },
  { key: "HienThi", name: "Hiển thị" },
  { key: "TenCapQuanLy", name: "Cấp quản lý" },
  { key: "NamBatDau", name: "Năm bắt đầu" },
  { key: "TenDoiTuongThongBao", name: "Tên đối tượng thông báo" },
];

const { TextArea } = Input;
const { TreeNode } = TreeSelect;
// import PropTypes from 'prop-types';
// import styled from 'styled-components';

/* eslint-disable react/prefer-stateless-function */
class GoModal extends React.Component {
  constructor(props) {
    super(props);
    this.submitBtn = React.createRef();
    this.state = {
      confirmLoading: false,
      data: {
        TenThongBao: null,
        NoiDung: null,
        ThoiGianBatDau: null,
        ThoiGianKetThuc: null,
        HienThi: true,
        CapQuanLy: null,
        NamBatDau: null,
        DoiTuongThongBao: [],
      },
      files: [],
      FileDinhKemOld: [],
      danhSachCanBo: [],
      treeValue: [],
      lichSuChinhSua: [],
    };
  }
  handleTreeChange = (value, label, extra) => {
    this.setState({ treeValue: value });
  };

  componentDidMount() {
    const { danhSachCanBo } = this.state;
    if (this.props.data) {
      const data = { ...this.state.data, ...this.props.data };
      // const treeValue = this.props.data.DoiTuongThongBao.map((item) => JSON.stringify({ CanBoID: item.CanBoID, CoQuanID: item.CoQuanID }));
      this.setState({ data: data, FileDinhKemOld: data.FileDinhKem });
    }
    api.danhSachTaiKhoan({ PageSize: 999999 }).then((res) => {
      if (res) {
        if (res.data && res.data.Status) {
          const danhSachCanBo = res.data.Data;
          let treeValue = [];
          if (this.props.data) {
            const grouped = lodash.groupBy(this.props.data.DoiTuongThongBao, "CoQuanID");

            Object.keys(grouped).forEach((item) => {
              const coquan = danhSachCanBo[0].Children.find((d) => {
                // console.log(d);
                return d.Id == item;
              });

              if (coquan && coquan.Children.length === grouped[item].length) {
                treeValue.push(JSON.stringify({ CanBoID: Number(item), CoQuanID: 0 }));
              } else {
                const selected = grouped[item].map((canbo) => JSON.stringify({ CanBoID: canbo.CanBoID, CoQuanID: canbo.CoQuanID }));
                treeValue = [...treeValue, ...selected];
              }
            });
            // console.log(treeValue.length, danhSachCanBo[0].Children.length);
            if (treeValue.length === danhSachCanBo[0].Children.length) {
              treeValue = [JSON.stringify({ CanBoID: 0, CoQuanID: 0 })];
            }
          }

          this.setState({ danhSachCanBo, treeValue });
        } else {
          message.error("Lấy danh sách cán bộ thất bại");
        }
      } else {
        message.error("Lấy danh sách cán bộ thất bại");
      }
    });
    if (this.props.data && this.props.data.ThongBaoID) {
      api.GetByID({ ThongBaoID: this.props.data.ThongBaoID }).then((res) => {
        this.setState({ lichSuChinhSua: res.data.Data.LichSuChinhSuaThongBao });
      });
    }
  }

  handleDeleteFile = (FileDinhKemID) => {
    Modal.confirm({
      content: "Bạn có muốn xoá file đính kèm này?",
      cancelText: "Huỷ",
      okText: "Đồng ý",
      onOk: () => {
        api.xoaFileDinhKem({ FileDinhKemID }).then((res) => {
          if (!res || !res.data || res.data.Status !== 1) {
            this.setState({ confirmLoading: false });
            message.error(`${res && res.data ? res.data.Message : "Lỗi hệ thống"}`);
          } else {
            message.success("Xoá thành công");
            const { FileDinhKemOld } = this.state;
            FileDinhKemOld.splice(
              FileDinhKemOld.findIndex((d) => d.FileDinhKemID === FileDinhKemID),
              1
            );
            this.setState({ FileDinhKemOld });
          }
        });
      },
    });
  };

  handleCancel = () => {
    this.props.onClose();
  };
  handleOk = () => {
    this.submitBtn.current.click();
  };
  handleSubmit = () => {
    // console.log(this.state.data);
    // return;
    this.setState({ confirmLoading: true });
    const { data, treeValue, danhSachCanBo } = this.state;

    const newData = lodash.cloneDeep(this.state.data);

    if ((!newData.CapQuanLy || newData.CapQuanLy === 0) && !newData.NamBatDau) {
      newData.DoiTuongThongBao = [];
      treeValue.forEach((itemStr) => {
        const item = JSON.parse(itemStr);

        if (item.CoQuanID === 0 && item.CanBoID === 0) {
          danhSachCanBo[0].Children.forEach((coquan) => {
            if (coquan.Children.length === 0) {
              newData.DoiTuongThongBao.push({ CoQuanID: coquan.DepartmentId, CanBoID: coquan.Id });
            } else {
              coquan.Children.forEach((canbo) => {
                const { Id, DepartmentId } = canbo;
                newData.DoiTuongThongBao.push({ CoQuanID: DepartmentId, CanBoID: Id });
              });
            }
          });
        } else if (item.CoQuanID === 0 && item.CanBoID !== 0) {
          const coquan = danhSachCanBo[0].Children.find((d) => d.Id === item.CanBoID);
          if (coquan.Children.length === 0) {
            newData.DoiTuongThongBao.push({ CoQuanID: coquan.DepartmentId, CanBoID: coquan.Id });
          } else {
            coquan.Children.forEach((canbo) => {
              const { Id, DepartmentId } = canbo;
              newData.DoiTuongThongBao.push({ CoQuanID: DepartmentId, CanBoID: Id });
            });
          }
        } else {
          newData.DoiTuongThongBao.push(item);
        }
      });
    }

    api.EditThongBao(newData).then((res) => {
      if (!res || !res.data || res.data.Status !== 1) {
        message.error(`${res && res.data ? res.data.Message : "Lỗi hệ thống"}`);
        this.setState({ confirmLoading: false });
      } else {
        const formData = new FormData();
        this.state.files.forEach((element, index) => {
          formData.append("files", this.state.files[index]);
        });
        formData.append("NoiDung", JSON.stringify({ LoaiFile: 19, NghiepVuID: res.data.Data, NoiDung: this.state.data.NoiDung }));

        api.themFileDinhKem(formData).then((res) => {
          if (!res || !res.data || res.data.Status !== 1) {
            message.error(`${res && res.data ? res.data.Message : "Thêm thông báo thành công, đính kèm file thất bại"}`);
          } else {
            message.success(`${this.props.data ? "Cập nhật" : "Thêm mới"} thành công`);
          }
          this.props.onClose();
        });
      }
    });

    return;
  };
  handleInputChange = (name) => (event) => {
    const { data } = this.state;
    data[name] = event && event.target ? event.target.value : event;
    this.setState({
      data,
    });
    if (name === "CapQuanLy") {
      this.getDanhSachCanBoTheoCapQL();
    }
  };
  getDanhSachCanBoTheoCapQL = () => {
    const { data } = this.state;
    const { NamBatDau, CapQuanLy } = this.state.data;
    if (!NamBatDau && !CapQuanLy) {
      data.DoiTuongThongBao = [];
      this.setState({ data });
      return;
    }
    api.danhSachCanBoTheoCapQuanLy({ NamBatDau, CapQuanLy }).then((res) => {
      const { data } = this.state;
      if (res.data.Status === 1) {
        // data.DoiTuongThongBao = res.data.Data;
        data.DoiTuongThongBao = lodash.unionBy(res.data.Data, "CanBoID");
        this.setState({ data });
      } else {
        data.DoiTuongThongBao = [];
        message.error("Lấy danh sách đối tượng thông báo theo cấp quản lý thất bại.");
        this.setState({ data });
      }
    });
  };
  renderTreeNode = (data = []) => {
    return data.map((item) => {
      const value = { CanBoID: item.Id, CoQuanID: item.DepartmentId };

      // this.flatTree.push(item);
      if (item.Children) {
        return (
          <TreeNode
            value={JSON.stringify(value)}
            title={
              // item.Name
              <Tooltip title={item.Name} mouseEnterDelay={0.5}>
                <span className="d-inline-block text-truncate " style={{ width: "100%" }}>
                  {item.Name}
                </span>
              </Tooltip>
            }
            searchKey={item.Name}
            key={JSON.stringify(value)}
          >
            {this.renderTreeNode(item.Children)}
          </TreeNode>
        );
      } else {
        return (
          <TreeNode
            value={JSON.stringify(value)}
            title={
              <Tooltip title={item.Name} mouseEnterDelay={0.5}>
                <span className="d-inline-block text-truncate" style={{ width: 240 }}>
                  {item.Name}
                </span>
              </Tooltip>
            }
            searchKey={item.Name}
            key={JSON.stringify(value)}
          ></TreeNode>
        );
      }
    });
  };

  render() {
    const { confirmLoading, data, lichSuChinhSua } = this.state;
    const lichSuChinhSuaTable = lichSuChinhSua.slice(1, lichSuChinhSua.length);
    // console.log(lichSuChinhSua, lichSuChinhSuaTable);
    const columns = [
      {
        title: "STT",
        key: "stt",
        dataIndex: "stt",
        align: "center",
        width: "5%",
        render: (text, record, index) => <span>{index + 1}</span>,
      },
      {
        title: "Người chỉnh sửa ",
        dataIndex: "TenNguoiChinhSua",
        key: "TenNguoiChinhSua",
        align: "center",
        width: "20%",
      },
      {
        title: "Ngày chỉnh sửa",
        width: "20%",
        align: "center",
        render: (text, record, index) => <span>{moment(record.NgayChinhSua).format("DD/MM/YYYY ")}</span>,
      },
      {
        title: "Nội dung trước khi chỉnh sửa",
        align: "center",
        width: "45%",
        render: (text, record, index) => {
          const diff = [];
          fields.forEach((item) => {
            if (record[item.key] !== lichSuChinhSua[index][item.key]) {
              if (item.key === "ThoiGianBatDau" || item.key === "ThoiGianKetThuc") {
                diff.push(`${item.name}: ${moment(lichSuChinhSua[index][item.key]).format("DD/MM/YYYY")}`);
                return;
              }
              if (item.key === "HienThi") {
                diff.push(`${item.name}: ${lichSuChinhSua[index][item.key] ? "Có" : "Không"}`);
                return;
              }
              diff.push(`${item.name}: ${lichSuChinhSua[index][item.key] ? lichSuChinhSua[index][item.key] : "Không có"}`);
            }
          });

          return (
            <div className="text-left">
              {diff.map((item) => (
                <div>- {item}</div>
              ))}
            </div>
          );
        },
      },
    ];
    return (
      <Modal confirmLoading={confirmLoading} width={960} title={"Nội dung thông báo"} visible={true} onOk={this.handleOk} onCancel={this.handleCancel} cancelText="Huỷ" okText="Lưu">
        <ValidatorForm ref="form" onSubmit={this.handleSubmit}>
          <div className="row">
            <div className="col-12 my-1  ">
              <div className="row align-items-center">
                <div className=" col-lg-2 ">
                  Tên thông báo <span style={{ color: "red" }}>*</span>
                </div>
                <div className="col-lg-10 ">
                  <GoInput value={data.TenThongBao} onChange={this.handleInputChange("TenThongBao")} placeholder="" allowClear validators={["required"]} errorMessages={["Nội dung bắt buộc"]} />
                </div>
              </div>
            </div>
            <div className="col-12 my-1  ">
              <div className="row align-items-center">
                <div className=" col-lg-2 ">
                  Nội dung <span style={{ color: "red" }}>*</span>
                </div>
                <div className="col-lg-10 ">
                  <GoInput isTextArea value={data.NoiDung} onChange={this.handleInputChange("NoiDung")} placeholder="" allowClear validators={["required"]} errorMessages={["Nội dung bắt buộc"]} />
                </div>
              </div>
            </div>
            <div className="col-6 my-1  ">
              <div className="row align-items-center">
                <div className=" col-lg-4 ">Cấp quản lý</div>
                <div className="col-lg-8 ">
                  <TreeSelectWithApi
                    apiConfig={{
                      api: api.danhSachCapDeTai,
                      valueField: "ID",
                      nameField: "Name",
                    }}
                    placeholder="Cấp quản lý"
                    onChange={this.handleInputChange("CapQuanLy")}
                    value={this.state.data.CapQuanLy}
                  />
                </div>
              </div>
            </div>
            <div className="col-6 my-1  ">
              <div className="row align-items-center">
                <div className=" col-lg-4 ">Năm bắt đầu</div>
                <div className="col-lg-8 ">
                  <DatePicker4
                    picker="year"
                    placeholder="Chọn năm"
                    onChange={(value) => {
                      const { data } = this.state;
                      data.NamBatDau = value ? value.format("YYYY") : value;
                      this.setState({ data });
                      this.getDanhSachCanBoTheoCapQL();
                    }}
                    value={this.state.data.NamBatDau ? moment(this.state.data.NamBatDau, "YYYY") : this.state.data.NamBatDau}
                  ></DatePicker4>
                </div>
              </div>
            </div>
            <div className="col-12 my-1  ">
              <div className="row align-items-center">
                <div className=" col-lg-2 ">Đối tượng thông báo</div>
                <div className="col-lg-10 ">
                  {(this.state.data.CapQuanLy && this.state.data.CapQuanLy !== 0) || this.state.data.NamBatDau ? (
                    <div>
                      {this.state.data.DoiTuongThongBao.map((item) => (
                        <Tag className="my-1">{item.TenCanBo || item.CanBoID}</Tag>
                      ))}
                    </div>
                  ) : (
                    <TreeSelect
                      value={this.state.treeValue}
                      showCheckedStrategy={TreeSelect.SHOW_PARENT}
                      onChange={this.handleTreeChange}
                      multiple
                      treeCheckable
                      dropdownStyle={{ maxWidth: 400, overflowX: "hidden", maxHeight: 400 }}
                      allowClear
                      showSearch
                      filterTreeNode
                      treeNodeFilterProp="searchKey"
                    >
                      {this.renderTreeNode(this.state.danhSachCanBo)}
                    </TreeSelect>
                  )}
                </div>
              </div>
            </div>
            <div className="col-12 my-1  ">
              <div className="row align-items-center">
                <div className=" col-lg-2 ">
                  Thời gian <span style={{ color: "red" }}>*</span>
                </div>
                <div className="col-lg-5 ">
                  <GoDatePicker
                    onChange={(date) => {
                      const { data } = this.state;

                      data.ThoiGianBatDau = date ? date.startOf("day").format("YYYY-MM-DD") : null;
                      this.setState({ data });
                    }}
                    placeholder=""
                    value={data.ThoiGianBatDau ? moment(data.ThoiGianBatDau, "YYYY-MM-DD") : null}
                    format={"DD/MM/YYYY"}
                    validators={["required"]}
                    errorMessages={["Nội dung bắt buộc"]}
                  />
                </div>
                <div className="col-lg-5 ">
                  <GoDatePicker
                    onChange={(date) => {
                      const { data } = this.state;

                      data.ThoiGianKetThuc = date ? date.endOf("day").format("YYYY-MM-DD") : null;
                      this.setState({ data });
                    }}
                    placeholder=""
                    value={data.ThoiGianKetThuc ? moment(data.ThoiGianKetThuc, "YYYY-MM-DD") : null}
                    format={"DD/MM/YYYY"}
                    validators={["required"]}
                    errorMessages={["Nội dung bắt buộc"]}
                  />
                </div>
              </div>
            </div>
            <div className="col-12 my-1  ">
              <div className="row align-items-center">
                <div className=" col-lg-2 ">Hiển thị</div>
                <div className="col-lg-10 ">
                  <Checkbox
                    checked={data.HienThi}
                    onChange={(event) => {
                      const { data } = this.state;
                      data.HienThi = event.target.checked;
                      this.setState({ data });
                    }}
                  />
                </div>
              </div>
            </div>
            <div className="col-12 my-1">
              <div className=" row align-items-center">
                <div className=" col-lg-2">File đính kèm</div>
                <div className="col-lg-10 ">
                  <Upload
                    fileList={this.state.files}
                    multiple
                    beforeUpload={(file) => {
                      return false;
                    }}
                    onChange={async ({ file, fileList }) => {
                      const { files } = this.state;
                      if (file.status === "removed") {
                        const fileIndex = files.findIndex((d) => d.uid === file.uid);
                        files.splice(fileIndex, 1);
                        this.setState({ files });
                        return;
                      }
                      const result = await checkFilesSize(file);
                      if (!result.valid) {
                        message.error(`File đính kèm phải nhỏ hơn ${result.limitFileSize} MB. (${file.name})`);
                        return;
                      }
                      files.push(file);

                      this.setState({ files });
                    }}
                  >
                    <Button>
                      <Icon type="upload" /> Chọn file
                    </Button>
                  </Upload>
                  {this.state.FileDinhKemOld.map((item) => (
                    <div className="mx-1 my-1 file-upload-item d-block">
                      <Icon type="paper-clip"></Icon>
                      <span className="mx-2">{item.TenFileGoc}</span>
                      <span className="float-right delete-icon pointer" onClick={() => this.handleDeleteFile(item.FileDinhKemID)}>
                        <Icon type="delete"></Icon>
                      </span>
                      <div className="clearfix"></div>
                    </div>
                  ))}
                </div>
              </div>
            </div>
          </div>
          <button className="d-none" ref={this.submitBtn} type="submit">
            submit
          </button>
          <div className="custom-collapse ">
            <Collapse defaultActiveKey={["0"]} expandIconPosition={"right"}>
              <Collapse.Panel header="Lịch sử chỉnh sửa" key="0">
                <Table id="table" columns={columns} rowKey="DeTaiID" dataSource={lichSuChinhSuaTable} bordered={true} size="small" locale={{ emptyText: "Không có dữ liệu" }} />
              </Collapse.Panel>
            </Collapse>
          </div>
        </ValidatorForm>
      </Modal>
    );
  }
}
export default GoModal;

function walkTree(data) {}
