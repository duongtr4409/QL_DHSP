import React from "react";
import moment from "moment";
import { Table, Icon, Button, Modal, message, Row, Radio, Tabs } from "antd";
import { ModalAddEditBaiBaoTapChi } from "./ModalAddEditBaiBaoTapChi";
import { ModalAddEditSachChuyenKhao } from "./ModalAddEditSachChuyenKhao";
import { ModalAddEditSanPhamDaoTao } from "./ModalAddEditSanPhamDaoTao";
import { ModalAddEditKetQuaNghienCuu } from "./ModalAddEditKetQuaNghienCuu";
import {
  renderBaiBaoTapChi,
  renderCacMonGiangDay,
  renderDuAnDeTai,
  renderGiaiThuongKhoaHoc,
  renderHoatDongKhoaHoc,
  renderKetQuaNghienCuu,
  renderNgoaiNgu,
  renderQuaTrinhCongTac,
  renderQuaTrinhDaoTao,
  renderSachChuyenKhao,
  renderVanBangChungChi,
  logoHNUEBase64,
  renderSanPhamDaoTao,
  renderContentBaiBao,
  renderContentSachChuyenKhao,
  renderContentKetQuaNghienCuu,
} from "./renderFunction";
import api from "../../config";
import apiConfig from "../../../DataCoreAPI/config";
import { divide } from "lodash";

const { TabPane } = Tabs;
export default class TableData extends React.Component {
  state = {
    LoaiBaiBao: 1,
    loading: false,
  };
  modal = null;
  componentDidMount() {
    Promise.all([
      api.danhSachAllCanBo({ PageSize: 99999 }).then((res) => {
        if (res.data.Status === 1) {
          return res.data.Data;
        }
        return [];
      }),
      apiConfig.DanhSachNhiemVu2({}).then((res) => {
        if (res.data.Status === 1) {
          return res.data.Data;
        }
        return [];
      }),
      apiConfig.DanhSachPhongBan({ type: 0 }).then((res) => {
        if (res.data.Status === 1) {
          return res.data.Data;
        }
        return [];
      }),
    ]).then((res) => {
      const [DanhSachCanBo, DanhSachLoaiNhiemVu, DanhSachPhongBan] = res;
      this.setState({ DanhSachCanBo, DanhSachLoaiNhiemVu, DanhSachPhongBan });
    });
  }
  onCloseModal = () => {
    this.modal.destroy();
    this.modal = null;
  };
  openModalAdd = (key, data) => {
    let content = null;
    const { loading } = this.state;
    switch (key) {
      case 7:
        content = (
          <ModalAddEditBaiBaoTapChi
            onCreate={(value) => {
              let data = value;

              this.dialogRef.setConfirmLoading(true);
              data.LoaiThongTin = key;
              data.DeTaiID = this.props.DeTaiID;

              api.chinhSuaThongTinChiTiet(value).then((res) => {
                if (res.data.Status === 1) {
                  message.success("Thêm mới thành công");
                  this.props.refresh();
                  this.modal.destroy();
                } else {
                  this.dialogRef.setConfirmLoading(false);
                  message.error(res.data.Message);
                }
              });
            }}
            wrappedComponentRef={(ref) => (this.dialogRef = ref)}
            loading={loading}
            dataEdit={{}}
            onCancel={this.onCloseModal}
            DanhSachLoaiNhiemVu={this.state.DanhSachLoaiNhiemVu || []}
            DanhSachCanBo={this.state.DanhSachCanBo || []}
            DanhSachPhongBan={this.state.DanhSachPhongBan || []}
          ></ModalAddEditBaiBaoTapChi>
        );
        break;
      case 8:
        content = (
          <ModalAddEditKetQuaNghienCuu
            onCreate={(value) => {
              let data = value;

              this.dialogRef.setConfirmLoading(true);
              data.LoaiThongTin = key;
              data.DeTaiID = this.props.DeTaiID;

              api.chinhSuaThongTinChiTiet(value).then((res) => {
                if (res.data.Status === 1) {
                  message.success("Thêm mới thành công");
                  this.props.refresh();
                  this.modal.destroy();
                } else {
                  this.dialogRef.setConfirmLoading(false);
                  message.error(res.data.Message);
                }
              });
            }}
            wrappedComponentRef={(ref) => (this.dialogRef = ref)}
            loading={loading}
            dataEdit={{}}
            onCancel={this.onCloseModal}
            DanhSachLoaiNhiemVu={this.state.DanhSachLoaiNhiemVu || []}
            DanhSachCanBo={this.state.DanhSachCanBo || []}
            DanhSachPhongBan={this.state.DanhSachPhongBan || []}
          ></ModalAddEditKetQuaNghienCuu>
        );
        break;

      case 9:
        content = (
          <ModalAddEditSachChuyenKhao
            onCreate={(value) => {
              // console.log(value);
              // return;
              let data = value;

              this.dialogRef.setConfirmLoading(true);
              data.LoaiThongTin = key;
              data.DeTaiID = this.props.DeTaiID;

              api.chinhSuaThongTinChiTiet(value).then((res) => {
                if (res.data.Status === 1) {
                  message.success("Thêm mới thành công");
                  this.props.refresh();
                  this.modal.destroy();
                } else {
                  this.dialogRef.setConfirmLoading(false);
                  message.error(res.data.Message);
                }
              });
            }}
            wrappedComponentRef={(ref) => (this.dialogRef = ref)}
            loading={loading}
            dataEdit={{}}
            onCancel={this.onCloseModal}
            DanhSachLoaiNhiemVu={this.state.DanhSachLoaiNhiemVu || []}
            DanhSachCanBo={this.state.DanhSachCanBo || []}
            DanhSachPhongBan={this.state.DanhSachPhongBan || []}
          ></ModalAddEditSachChuyenKhao>
        );
        break;
      case 10:
        content = (
          <ModalAddEditSanPhamDaoTao
            onCreate={(value) => {
              // console.log(value);
              // return;
              let data = value;

              this.dialogRef.setConfirmLoading(true);
              data.LoaiThongTin = key;
              data.DeTaiID = this.props.DeTaiID;

              api.chinhSuaThongTinChiTiet(value).then((res) => {
                if (res.data.Status === 1) {
                  message.success("Thêm mới thành công");
                  this.props.refresh();
                  this.modal.destroy();
                } else {
                  this.dialogRef.setConfirmLoading(false);
                  message.error(res.data.Message);
                }
              });
            }}
            wrappedComponentRef={(ref) => (this.dialogRef = ref)}
            loading={loading}
            dataEdit={{}}
            onCancel={this.onCloseModal}
            DanhSachLoaiNhiemVu={this.state.DanhSachLoaiNhiemVu || []}
            DanhSachCanBo={this.state.DanhSachCanBo || []}
            DanhSachPhongBan={this.state.DanhSachPhongBan || []}
          ></ModalAddEditSanPhamDaoTao>
        );
        break;
      default:
        break;
    }

    this.modal = Modal.confirm({
      icon: <i></i>,
      content,
      footer: null,
    });
  };

  openModalEdit = (index, key) => {
    let dataEdit = {};
    let content = null;
    switch (key) {
      case 7: {
        const { LoaiBaiBao } = this.state;
        const BaiBaoTapChi = this.props.data.BaiBaoTapChi || [];
        dataEdit = BaiBaoTapChi.filter((item) => item.LoaiBaiBao === LoaiBaiBao)[index];
        content = (
          <ModalAddEditBaiBaoTapChi
            onCreate={(value) => {
              let data = value;
              this.dialogRef.setConfirmLoading(true);
              data.LoaiThongTin = key;
              data.DeTaiID = this.props.DeTaiID;
              data.ChiTietDeTaiID = dataEdit.ChiTietDeTaiID;
              api.chinhSuaThongTinChiTiet(value).then((res) => {
                if (res.data.Status === 1) {
                  message.success("cập nhật thành công");
                  this.props.refresh();
                  this.modal.destroy();
                } else {
                  this.dialogRef.setConfirmLoading(false);
                  message.error(res.data.Message);
                }
              });
            }}
            actions="edit"
            wrappedComponentRef={(ref) => (this.dialogRef = ref)}
            dataEdit={dataEdit}
            onCancel={this.onCloseModal}
            DanhSachLoaiNhiemVu={this.state.DanhSachLoaiNhiemVu || []}
            DanhSachCanBo={this.state.DanhSachCanBo || []}
            DanhSachPhongBan={this.state.DanhSachPhongBan || []}
          ></ModalAddEditBaiBaoTapChi>
        );

        break;
      }
      case 8: {
        const KetQuaNghienCuuCongBo = this.props.data.KetQuaNghienCuuCongBo || [];
        dataEdit = KetQuaNghienCuuCongBo[index];
        content = (
          <ModalAddEditKetQuaNghienCuu
            onCreate={(value) => {
              let data = value;
              this.dialogRef.setConfirmLoading(true);
              data.LoaiThongTin = key;
              data.DeTaiID = this.props.DeTaiID;
              data.ChiTietDeTaiID = dataEdit.ChiTietDeTaiID;
              api.chinhSuaThongTinChiTiet(value).then((res) => {
                if (res.data.Status === 1) {
                  message.success("cập nhật thành công");
                  this.props.refresh();
                  this.modal.destroy();
                } else {
                  this.dialogRef.setConfirmLoading(false);
                  message.error(res.data.Message);
                }
              });
            }}
            actions="edit"
            wrappedComponentRef={(ref) => (this.dialogRef = ref)}
            dataEdit={dataEdit}
            onCancel={this.onCloseModal}
            DanhSachLoaiNhiemVu={this.state.DanhSachLoaiNhiemVu || []}
            DanhSachCanBo={this.state.DanhSachCanBo || []}
            DanhSachPhongBan={this.state.DanhSachPhongBan || []}
          ></ModalAddEditKetQuaNghienCuu>
        );

        break;
      }

      case 9:
        {
          const SachChuyenKhao = this.props.data.SachChuyenKhao || [];

          dataEdit = SachChuyenKhao[index];
          content = (
            <ModalAddEditSachChuyenKhao
              onCreate={(value) => {
                let data = value;
                this.dialogRef.setConfirmLoading(true);
                data.LoaiThongTin = key;
                data.DeTaiID = this.props.DeTaiID;
                data.ChiTietDeTaiID = dataEdit.ChiTietDeTaiID;
                api.chinhSuaThongTinChiTiet(value).then((res) => {
                  if (res.data.Status === 1) {
                    message.success("cập nhật thành công");
                    this.props.refresh();
                    this.modal.destroy();
                  } else {
                    this.dialogRef.setConfirmLoading(false);
                    message.error(res.data.Message);
                  }
                });
              }}
              actions="edit"
              wrappedComponentRef={(ref) => (this.dialogRef = ref)}
              dataEdit={dataEdit}
              onCancel={this.onCloseModal}
              DanhSachLoaiNhiemVu={this.state.DanhSachLoaiNhiemVu || []}
              DanhSachCanBo={this.state.DanhSachCanBo || []}
              DanhSachPhongBan={this.state.DanhSachPhongBan || []}
            ></ModalAddEditSachChuyenKhao>
          );
        }

        break;
      case 10:
        {
          const SanPhamDaoTao = this.props.data.SanPhamDaoTao || [];
          dataEdit = SanPhamDaoTao[index];

          content = (
            <ModalAddEditSanPhamDaoTao
              onCreate={(value) => {
                let data = value;
                this.dialogRef.setConfirmLoading(true);
                data.LoaiThongTin = key;
                data.DeTaiID = this.props.DeTaiID;
                data.ChiTietDeTaiID = dataEdit.ChiTietDeTaiID;
                api.chinhSuaThongTinChiTiet(value).then((res) => {
                  if (res.data.Status === 1) {
                    message.success("cập nhật thành công");
                    this.props.refresh();
                    this.modal.destroy();
                  } else {
                    this.dialogRef.setConfirmLoading(false);
                    message.error(res.data.Message);
                  }
                });
              }}
              actions="edit"
              wrappedComponentRef={(ref) => (this.dialogRef = ref)}
              dataEdit={dataEdit}
              onCancel={this.onCloseModal}
              DanhSachLoaiNhiemVu={this.state.DanhSachLoaiNhiemVu || []}
              DanhSachCanBo={this.state.DanhSachCanBo || []}
              DanhSachPhongBan={this.state.DanhSachPhongBan || []}
            ></ModalAddEditSanPhamDaoTao>
          );
        }

        break;
      default:
        break;
    }
    this.modal = Modal.confirm({
      icon: <i></i>,
      content,
      footer: null,
    });
  };

  deleteData = (index, type) => {
    Modal.confirm({
      title: "Thông báo",
      content: "Xác nhận xóa thông tin ?",
      okText: "Có",
      cancelText: "Không",
      onOk: () => {
        api
          .xoaThongTinChiTiet({
            ChiTietDeTaiID: index,
          })
          .then((response) => {
            if (response.data.Status > 0) {
              message.destroy();
              message.success("Xóa thông tin thành công");
              this.props.refresh();
            } else {
              message.destroy();
              message.error(response.data.Message);
            }
          })
          .catch((error) => {
            message.destroy();
            message.error(error.toString());
          });
      },
    });
  };

  render() {
    const { LoaiBaiBao, DanhSachCanBo, DanhSachLoaiNhiemVu } = this.state;
    const BaiBaoTapChi = this.props.data.BaiBaoTapChi || [];
    const SachChuyenKhao = this.props.data.SachChuyenKhao || [];
    const SanPhamDaoTao = this.props.data.SanPhamDaoTao || [];
    const KetQuaNghienCuu = this.props.data.KetQuaNghienCuuCongBo || [];
    return (
      <div className="row justify-content-center align-item-center">
        <div className="col-12">
          <Tabs defaultActiveKey="1">
            <TabPane tab="Bài báo trên tạp chí KHCN" key="1">
              <div id={"baibaotapchi"} className={""}>
                <Radio.Group value={LoaiBaiBao} onChange={(value) => this.setState({ LoaiBaiBao: value.target.value })}>
                  <Radio value={1}>Bài báo quốc tế</Radio>
                  <Radio value={2}>Bài báo trong nước</Radio>
                  <Radio value={3}>Hội thảo quốc tế</Radio>
                  <Radio value={4}>Hội thảo trong nước</Radio>
                </Radio.Group>
                <div className="float-right">
                  <Button type={"primary"} style={{ marginLeft: 10, height: 30, width: 70 }} onClick={() => this.openModalAdd(7)} disabled={!this.props.role.edit}>
                    Thêm
                  </Button>
                </div>
                <div className="clearfix"></div>
              </div>
              {BaiBaoTapChi.filter((item) => item.LoaiBaiBao === LoaiBaiBao).length > 0 ? (
                renderBaiBaoTapChi(
                  BaiBaoTapChi.filter((item) => item.LoaiBaiBao === LoaiBaiBao),
                  this.openModalEdit,
                  this.deleteData
                )
              ) : (
                <div className="text-center">
                  <div className="row-empty">Không có dữ liệu</div>
                </div>
              )}
            </TabPane>
            <TabPane tab="Sách chuyên khảo đã xuất bản" key="2">
              <div className={"action-btn text-right"}>
                <Button type={"primary"} style={{ marginLeft: 10, height: 30, width: 70 }} onClick={() => this.openModalAdd(9)} disabled={!this.props.role.edit}>
                  Thêm
                </Button>
              </div>
              {SachChuyenKhao.length > 0 ? (
                renderSachChuyenKhao(SachChuyenKhao, this.openModalEdit, this.deleteData, DanhSachCanBo)
              ) : (
                <div className="text-center">
                  <div className="row-empty">Không có dữ liệu</div>
                </div>
              )}
            </TabPane>
            <TabPane tab="Kết quả đào tạo" key="3">
              <div className={"action-btn text-right mb-2"}>
                <Button type={"primary"} style={{ marginLeft: 10, height: 30, width: 70 }} onClick={() => this.openModalAdd(10)} disabled={!this.props.role.edit}>
                  Thêm
                </Button>
              </div>

              {SanPhamDaoTao.length > 0 ? (
                renderSanPhamDaoTao(SanPhamDaoTao, this.openModalEdit, this.deleteData)
              ) : (
                <div className="text-center">
                  <div className="row-empty">Không có dữ liệu</div>
                </div>
              )}
            </TabPane>
            <TabPane tab=" Kết quả nghiên cứu đã công bố hoặc đăng ký khác" key="4">
              <div className={"action-btn text-right"}>
                <Button type={"primary"} style={{ marginLeft: 10, height: 30, width: 70 }} onClick={() => this.openModalAdd(8)} disabled={!this.props.role.add}>
                  Thêm
                </Button>
              </div>
              {KetQuaNghienCuu.length > 0 ? (
                renderKetQuaNghienCuu(KetQuaNghienCuu, this.openModalEdit, this.deleteData, DanhSachCanBo)
              ) : (
                <div className="text-center">
                  <div className="row-empty">Không có dữ liệu</div>
                </div>
              )}
            </TabPane>
          </Tabs>
        </div>
      </div>
    );
  }
}
