import React, {Component} from "react";
import {connect} from "react-redux";
import {
  Anchor, Avatar, Checkbox, Input, Layout, message, Radio,
  Row, Tooltip, Upload, Col, Icon, Modal, Spin, Popover
} from "antd";
import LayoutWrapper from "../../../components/utility/layoutWrapper";
import PageHeader from "../../../components/utility/pageHeader";
import Box from "../../../components/utility/box";
import PageAction from "../../../components/utility/pageAction";
import Button from "../../../components/uielements/button";
import actions from "../../redux/ChiTietNhaKhoaHoc/actions";
import queryString from 'query-string';
import api, {apiUrl} from "./config";
import {formDataCaller} from '../../../helpers/formDataCaller'
import './style.css';
import defaultAvatar from "../../../image/defaultAvatar.jpeg"
import defaultAvatar_Female from "../../../image/defaultAvatar_female.jpg";
import Datepicker from "../../../components/uielements/datePickerFormat";
import Select, {Option} from '../../../components/uielements/select4';
import moment from "moment";
import {Link} from 'react-router-dom';
import {LoaiFileDinhKem, LoaiThongTinNhaKhoaHoc} from '../../../settings/constants';
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
  renderContentSachChuyenKhao, renderContentKetQuaNghienCuu, renderSanPhamDaoTao2, renderKetQuaNghienCuu2
} from './renderFunction';
import {getRoleByKey, checkValidFileName} from '../../../helpers/utility';

import {ModalAddEditQuaTrinhDaoTao} from './ModalAddEditQuaTrinhDaoTao';
import {ModalAddEditQuaTrinhCongTac} from './ModalAddEditQuaTrinhCongTac';
import {ModalAddEditNgoaiNgu} from "./ModalAddEditNgoaiNgu";
import {ModalAddEditDuAnDeTai} from "./ModalAddEditDuAnDeTai";
import {ModalAddEditVanBangChungChi} from "./ModalAddEditVanBangChungChi";
import {ModalAddEditGiaiThuongKhoaHoc} from "./ModalAddEditGiaiThuongKhoaHoc";
import {ModalAddEditKetQuaNghienCuu} from "./ModalAddEditKetQuaNghienCuu";
import {ModalAddEditCacMonGiangDay} from "./ModalAddEditCacMonGiangDay";
import {ModalAddEditBaiBaoTapChi} from "./ModalAddEditBaiBaoTapChi";
import {ModalAddEditSachChuyenKhao} from "./ModalAddEditSachChuyenKhao";
import {ModalAddEditHoatDongKhoaHoc} from "./ModalAddEditHoatDongKhoaHocKhac";
import {ModalAddEditBaoCaoKhoaHoc} from "./ModalAddEditBaoCaoKhoaHoc";
import {ModalAddEditSanPhamDaoTao} from "./ModalAddEditSanPhamDaoTao";

import Prompt from "react-router/Prompt";
import ImgCrop from "antd-img-crop";

import Wrapper from "./LyLichKhoaHocWrapper";

const {Sider, Content} = Layout;
const {Group} = Radio;

// const {Option} = Select;

class ChiTietNhaKhoaHoc extends Component {
  constructor(props) {
    document.title = 'Thông tin nhà khoa học';
    super(props);
    const filterData = queryString.parse(this.props.location.search);
    this.formPrint = React.createRef();
    this.isBackButtonClicked = false;
    this.state = {
      modalKey: 0,
      actions: "",
      loading: false,
      filterData: filterData,
      errAPI: false,
      //
      ThongTinNhaKhoaHoc: {},
      ThongTinNhaKhoaHocStr: "",
      saveThongTinCaNhan: false,
      //
      QuaTrinhDaoTao: [],
      dataEditQuaTrinhDaoTao: {},
      visibleModalQuaTrinhDaoTao: false,
      //
      QuaTrinhCongTac: [],
      dataEditQuaTrinhCongTac: {},
      visibleModalQuaTrinhCongTac: false,
      //
      NgoaiNgu: [],
      dataEditNgoaiNgu: {},
      visibleModalNgoaiNgu: false,
      //
      VanBangChungChi: [],
      dataEditVanBangChungChi: {},
      visibleModalVanBangChungChi: false,
      //
      GiaiThuongKhoaHoc: [],
      dataEditGiaiThuongKhoaHoc: {},
      visibleModalGiaiThuongKhoaHoc: false,
      //
      HuongNghienCuuChinh: {},
      saveHuongNghienCuuChinh: false,
      //
      DuAnDeTai: [],
      dataEditDuAnDeTai: {},
      visibleModalDuAnDeTai: false,
      //
      BaiBaoTapChi: [],
      dataEditBaiBaoTapChi: {},
      visibleModalBaiBaoTapChi: false,
      LoaiBaiBao: 1,
      //
      BaoCaoKhoaHoc: [],
      dataEditBaoCaoKhoaHoc: {},
      visibleModalBaoCaoKhoaHoc: false,
      //
      KetQuaNghienCuu: [],
      dataEditKetQuaNghienCuu: {},
      visibleModalKetQuaNghienCuu: false,
      //
      SachChuyenKhao: [],
      dataEditSachChuyenKhao: {},
      visibleModalSachChuyenKhao: false,
      //
      SanPhamDaoTao: [],
      dataEditSanPhamDaoTao: {},
      visibleModalSanPhamDaoTao: false,
      //
      CacMonGiangDay: [],
      dataEditCacMonGiangDay: {},
      visibleModalCacMonGiangDay: false,
      //
      HoatDongKhoaHoc: [],
      dataEditHoatDongKhoaHoc: {},
      visibleModalHoatDongKhoaHoc: false,
      //
      disableEdit: false,
      disableDelete: false,
      disableAdd: false,
      loadingData: false,
      CanBoTrongTruong: false,
      LaCanBoDangNhap: false,
      //
    }
  }

  componentDidMount() {
    this.props.getInitData(this.state.filterData);
    console.log("log props: ", this.props);
    console.log("log state: ", this.state);
    this.getInfoNhaKhoaHoc();
  }

  getInfoNhaKhoaHoc = () => {
    const {filterData} = this.state;
    const roleCT = getRoleByKey('chi-tiet-nha-khoa-hoc');
    this.setState({loadingData: true});
    const user = JSON.parse(localStorage.getItem('user'));
    const CanBoID = user.CanBoID;
    api.ChiTietNhaKhoaHoc({CanBoID: 1, CoQuanID: filterData.CoQuanID}).then(response => {
      // api.ChiTietNhaKhoaHoc({CanBoID: filterData.CanBoID, CoQuanID: filterData.CoQuanID}).then(response => {
      if (response.data.Status > 0) {
        const ThongTinNhaKhoaHoc = response.data.Data;
        if (ThongTinNhaKhoaHoc && ThongTinNhaKhoaHoc.CanBoID > 0) {
          // console.log(ThongTinNhaKhoaHoc);
          ThongTinNhaKhoaHoc.NgaySinh = ThongTinNhaKhoaHoc.NgaySinh ? moment(ThongTinNhaKhoaHoc.NgaySinh) : null;
          if (!ThongTinNhaKhoaHoc.GioiTinh) {
            const GioiTinhStr = ThongTinNhaKhoaHoc.GioiTinhStr;
            ThongTinNhaKhoaHoc.GioiTinh = GioiTinhStr === "Nam" ? 1 : 0;
          }
          const ThongTinNhaKhoaHocStr = JSON.stringify(ThongTinNhaKhoaHoc);
          // const disableEdit = CanBoID != ThongTinNhaKhoaHoc.CanBoID;
          let disableEdit = !(roleCT && roleCT.edit);
          let disableDelete = !(roleCT && roleCT.delete);
          let disableAdd = !(roleCT && roleCT.add);
          const LaCanBoDangNhap = CanBoID === ThongTinNhaKhoaHoc.CanBoID;
          const CanBoTrongTruong = ThongTinNhaKhoaHoc.LaCanBoTrongTruong;
          if (CanBoTrongTruong) {
            ThongTinNhaKhoaHoc.CoQuanCongTac = "Trường Đại học Sư phạm Hà Nội";
            ThongTinNhaKhoaHoc.DiaChiCoQuan = "136 Xuân Thủy, Cầu Giấy, Hà Nội";
            if (LaCanBoDangNhap) {
              disableEdit = false;
              disableDelete = false;
              disableAdd = false;
              disableEdit = !(roleCT && roleCT.edit);
              disableDelete = !(roleCT && roleCT.delete);
              disableAdd = !(roleCT && roleCT.add);
            } else {
              disableEdit = true;
              disableDelete = true;
              disableAdd = true;
            }
          }
          const QuaTrinhDaoTao = ThongTinNhaKhoaHoc.QuaTrinhDaoTao || [];
          const QuaTrinhCongTac = ThongTinNhaKhoaHoc.QuaTrinhCongTac || [];
          const NgoaiNgu = ThongTinNhaKhoaHoc.NgoaiNgu || [];
          const VanBangChungChi = ThongTinNhaKhoaHoc.VanBangChungChi || [];
          const GiaiThuongKhoaHoc = ThongTinNhaKhoaHoc.GiaiThuongKhoaHoc || [];
          const DuAnDeTai = ThongTinNhaKhoaHoc.DuAnDeTai || [];
          const BaiBaoTapChi = ThongTinNhaKhoaHoc.BaiBaoTapChi || [];
          const KetQuaNghienCuu = ThongTinNhaKhoaHoc.KetQuaNghienCuu || [];
          const SachChuyenKhao = ThongTinNhaKhoaHoc.SachChuyenKhao || [];
          const SanPhamDaoTao = ThongTinNhaKhoaHoc.SanPhamDaoTao || [];
          const CacMonGiangDay = ThongTinNhaKhoaHoc.CacMonGiangDay || [];
          const HoatDongKhoaHoc = ThongTinNhaKhoaHoc.HoatDongKhoaHoc || [];
          const HuongNghienCuuChinh = ThongTinNhaKhoaHoc.HuongNghienCuuChinh || {};
          this.setState({
            ThongTinNhaKhoaHoc,
            ThongTinNhaKhoaHocStr,
            disableEdit,
            disableDelete,
            disableAdd,
            CanBoTrongTruong,
            QuaTrinhDaoTao,
            QuaTrinhCongTac,
            NgoaiNgu,
            VanBangChungChi,
            GiaiThuongKhoaHoc,
            DuAnDeTai,
            BaiBaoTapChi,
            KetQuaNghienCuu,
            SachChuyenKhao,
            HoatDongKhoaHoc,
            HuongNghienCuuChinh,
            CacMonGiangDay,
            SanPhamDaoTao,
            loadingData: false,
            errAPI: false
          });
        } else {
          Modal.error({
            title: 'Thông báo',
            content: 'Không tìm thấy kết quả thông tin nhà khoa học',
            okText: 'Đóng'
          });
          this.setState({disableEdit: true, loadingData: false, errAPI: true});
        }
      } else {
        Modal.error({
          title: 'Thông báo',
          content: 'Không tìm thấy kết quả thông tin nhà khoa học',
          okText: 'Đóng'
        });
        this.setState({disableEdit: true, loadingData: false, errAPI: true});
      }
    }).catch(error => {
      message.warn(error.toString());
      this.setState({disableEdit: true, loadingData: false, errAPI: true});
    })
  };

  renderEmpty = () => {
    return <div>
      <div className={'row-empty'}/>
      <div className={'row-empty'}>
        Không có thông tin
      </div>
    </div>
  };

  closeModal = (type) => {
    switch (type) {
      case 1: //Modal dào tạo
        this.setState({visibleModalQuaTrinhDaoTao: false});
        break;
      case 2:
        this.setState({visibleModalQuaTrinhCongTac: false});
        break;
      case 3:
        this.setState({visibleModalNgoaiNgu: false});
        break;
      case 4:
        this.setState({visibleModalVanBangChungChi: false});
        break;
      case 5:
        this.setState({visibleModalGiaiThuongKhoaHoc: false});
        break;
      case 6:
        this.setState({visibleModalDuAnDeTai: false});
        break;
      case 7:
        this.setState({visibleModalBaiBaoTapChi: false});
        break;
      case 8:
        this.setState({visibleModalKetQuaNghienCuu: false});
        break;
      case 9:
        this.setState({visibleModalSachChuyenKhao: false});
        break;
      case 10:
        this.setState({visibleModalCacMonGiangDay: false});
        break;
      case 11:
        this.setState({visibleModalHoatDongKhoaHoc: false});
        break;
      case 12:
        this.setState({visibleModalBaoCaoKhoaHoc: false});
        break;
      case 13:
        this.setState({visibleModalSanPhamDaoTao: false});
        break;
    }
  };

  openModalAdd = (type) => {
    let {modalKey} = this.state;
    modalKey++;
    this.setState({modalKey, actions: 'add', loading: false});
    switch (type) {
      case 1: //Modal dào tạo
        this.setState({visibleModalQuaTrinhDaoTao: true, dataEditQuaTrinhDaoTao: {}});
        break;
      case 2:
        this.setState({visibleModalQuaTrinhCongTac: true, dataEditQuaTrinhCongTac: {}});
        break;
      case 3:
        this.setState({visibleModalNgoaiNgu: true, dataEditNgoaiNgu: {}});
        break;
      case 4:
        this.setState({visibleModalVanBangChungChi: true, dataEditVanBangChungChi: {}});
        break;
      case 5:
        this.setState({visibleModalGiaiThuongKhoaHoc: true, dataEditGiaiThuongKhoaHoc: {}});
        break;
      case 6:
        this.setState({visibleModalDuAnDeTai: true, dataEditDuAnDeTai: {}});
        break;
      case 7:
        this.setState({visibleModalBaiBaoTapChi: true, dataEditBaiBaoTapChi: {}});
        break;
      case 8:
        this.setState({visibleModalKetQuaNghienCuu: true, dataEditKetQuaNghienCuu: {}});
        break;
      case 9:
        this.setState({visibleModalSachChuyenKhao: true, dataEditSachChuyenKhao: {}});
        break;
      case 10:
        this.setState({visibleModalCacMonGiangDay: true, dataEditCacMonGiangDay: {}});
        break;
      case 11:
        this.setState({visibleModalHoatDongKhoaHoc: true, dataEditHoatDongKhoaHoc: {}});
        break;
      case 12:
        this.setState({visibleModalBaoCaoKhoaHoc: true, dataEditBaoCaoKhoaHoc: {}});
        break;
      case 13:
        this.setState({visibleModalSanPhamDaoTao: true, dataEditSanPhamDaoTao: {}});
        break;
    }
  };

  openModalEdit = (index, type) => {
    const {disableEdit} = this.state;
    const {role} = this.props;
    if (disableEdit) {
      return;
    }
    let {modalKey} = this.state;
    modalKey++;
    this.setState({modalKey, actions: 'edit', loading: false});
    let dataEdit = {};
    switch (type) {
      case 1: //Modal dào tạo
        const {QuaTrinhDaoTao} = this.state;
        dataEdit = QuaTrinhDaoTao[index];
        this.setState({visibleModalQuaTrinhDaoTao: true, dataEditQuaTrinhDaoTao: dataEdit});
        break;
      case 2:
        const {QuaTrinhCongTac} = this.state;
        dataEdit = QuaTrinhCongTac[index];
        this.setState({visibleModalQuaTrinhCongTac: true, dataEditQuaTrinhCongTac: dataEdit});
        break;
      case 3:
        const {NgoaiNgu} = this.state;
        dataEdit = NgoaiNgu[index];
        this.setState({visibleModalNgoaiNgu: true, dataEditNgoaiNgu: dataEdit});
        break;
      case 4:
        const {VanBangChungChi} = this.state;
        dataEdit = VanBangChungChi[index];
        dataEdit.NgayCap = dataEdit.NgayCap ? moment(dataEdit.NgayCap) : null;
        this.setState({visibleModalVanBangChungChi: true, dataEditVanBangChungChi: dataEdit});
        break;
      case 5:
        const {GiaiThuongKhoaHoc} = this.state;
        dataEdit = GiaiThuongKhoaHoc[index];
        this.setState({visibleModalGiaiThuongKhoaHoc: true, dataEditGiaiThuongKhoaHoc: dataEdit});
        break;
      case 6:
        const {DuAnDeTai} = this.state;
        dataEdit = DuAnDeTai[index];
        this.setState({visibleModalDuAnDeTai: true, dataEditDuAnDeTai: dataEdit});
        break;
      case 7:
        const {BaiBaoTapChi, LoaiBaiBao} = this.state;
        dataEdit = BaiBaoTapChi.filter(item => item.LoaiBaiBao === LoaiBaiBao)[index];
        this.setState({visibleModalBaiBaoTapChi: true, dataEditBaiBaoTapChi: dataEdit});
        break;
      case 8:
        const {KetQuaNghienCuu} = this.state;
        dataEdit = KetQuaNghienCuu[index];
        this.setState({visibleModalKetQuaNghienCuu: true, dataEditKetQuaNghienCuu: dataEdit});
        break;
      case 9:
        const {SachChuyenKhao} = this.state;
        dataEdit = SachChuyenKhao[index];
        this.setState({visibleModalSachChuyenKhao: true, dataEditSachChuyenKhao: dataEdit});
        break;
      case 10:
        const {CacMonGiangDay} = this.state;
        dataEdit = CacMonGiangDay[index];
        this.setState({visibleModalCacMonGiangDay: true, dataEditCacMonGiangDay: dataEdit});
        break;
      case 11:
        const {HoatDongKhoaHoc} = this.state;
        dataEdit = HoatDongKhoaHoc[index];
        this.setState({visibleModalHoatDongKhoaHoc: true, dataEditHoatDongKhoaHoc: dataEdit});
        break;
      case 12:
        const {BaoCaoKhoaHoc} = this.state;
        dataEdit = BaoCaoKhoaHoc[index];
        this.setState({visibleModalBaoCaoKhoaHoc: true, dataEditBaoCaoKhoaHoc: dataEdit});
        break;
      case 13:
        const {SanPhamDaoTao} = this.state;
        dataEdit = SanPhamDaoTao[index];
        this.setState({visibleModalSanPhamDaoTao: true, dataEditSanPhamDaoTao: dataEdit});
        break;
    }
  };

  deleteData = (index, type) => {
    const {disableDelete} = this.state;
    const {role, roleQL} = this.props;
    if (disableDelete) {
      return;
    }
    Modal.confirm({
      title: 'Thông báo',
      content: 'Xác nhận xóa thông tin ?',
      okText: 'Có',
      cancelText: 'Không',
      onOk: () => {
        let CTNhaKhoaHocID = 0;
        let LoaiFile = 0;
        switch (type) {
          case 1: //Modal dào tạo
            const {QuaTrinhDaoTao} = this.state;
            CTNhaKhoaHocID = QuaTrinhDaoTao[index].CTNhaKhoaHocID;
            break;
          case 2:
            const {QuaTrinhCongTac} = this.state;
            CTNhaKhoaHocID = QuaTrinhCongTac[index].CTNhaKhoaHocID;
            break;
          case 3:
            const {NgoaiNgu} = this.state;
            CTNhaKhoaHocID = NgoaiNgu[index].CTNhaKhoaHocID;
            break;
          case 4:
            const {VanBangChungChi} = this.state;
            CTNhaKhoaHocID = VanBangChungChi[index].CTNhaKhoaHocID;
            break;
          case 5:
            const {GiaiThuongKhoaHoc} = this.state;
            CTNhaKhoaHocID = GiaiThuongKhoaHoc[index].CTNhaKhoaHocID;
            break;
          case 6:
            const {DuAnDeTai} = this.state;
            CTNhaKhoaHocID = DuAnDeTai[index].CTNhaKhoaHocID;
            break;
          case 7:
            const {BaiBaoTapChi, LoaiBaiBao} = this.state;
            CTNhaKhoaHocID = BaiBaoTapChi.filter(item => item.LoaiBaiBao === LoaiBaiBao)[index].CTNhaKhoaHocID;
            break;
          case 8:
            const {KetQuaNghienCuu} = this.state;
            CTNhaKhoaHocID = KetQuaNghienCuu[index].CTNhaKhoaHocID;
            break;
          case 9:
            const {SachChuyenKhao} = this.state;
            CTNhaKhoaHocID = SachChuyenKhao[index].CTNhaKhoaHocID;
            break;
          case 10:
            const {CacMonGiangDay} = this.state;
            CTNhaKhoaHocID = CacMonGiangDay[index].CTNhaKhoaHocID;
            LoaiFile = LoaiFileDinhKem.CacMonGiangDay;
            break;
          case 11:
            const {HoatDongKhoaHoc} = this.state;
            CTNhaKhoaHocID = HoatDongKhoaHoc[index].CTNhaKhoaHocID;
            LoaiFile = LoaiFileDinhKem.HoatDongKhoaHoc;
            break;
          case 12:
            const {BaoCaoKhoaHoc} = this.state;
            CTNhaKhoaHocID = BaoCaoKhoaHoc[index].CTNhaKhoaHocID;
            break;
          case 13:
            const {SanPhamDaoTao} = this.state;
            CTNhaKhoaHocID = SanPhamDaoTao[index].CTNhaKhoaHocID;
            break;
        }
        const ChiTiet = {
          CTNhaKhoaHocID: CTNhaKhoaHocID,
          LoaiFile: LoaiFile,
          HoatDongKhoaHocID: CTNhaKhoaHocID
        };
        api.XoaThongTinChiTiet(ChiTiet)
          .then(response => {
            if (response.data.Status > 0) {
              message.destroy();
              message.success('Xóa thông tin thành công');
              this.getInfoNhaKhoaHoc();
            } else {
              message.destroy();
              message.error(response.data.Message);
            }
          }).catch(error => {
          message.destroy();
          message.error(error.toString());
        });
      }
    });
  };

  changeInfoNhaKhoaHoc = (value, field) => {
    const {ThongTinNhaKhoaHoc, ThongTinNhaKhoaHocStr} = this.state;
    ThongTinNhaKhoaHoc[field] = value;
    const tempThongTinKhoaHocStr = JSON.stringify(ThongTinNhaKhoaHoc);
    const saveThongTinCaNhan = tempThongTinKhoaHocStr !== ThongTinNhaKhoaHocStr;
    this.setState({ThongTinNhaKhoaHoc, saveThongTinCaNhan})
  };

  changeHuongNghienCuuChinh = (value) => {
    const {HuongNghienCuuChinh} = this.state;
    const saveHuongNghienCuuChinh = value !== HuongNghienCuuChinh.TenHuongNghienCuuChinh;
    HuongNghienCuuChinh.TenHuongNghienCuuChinh = value;
    this.setState({HuongNghienCuuChinh, saveHuongNghienCuuChinh});
  };

  saveThongTinCaNhan = (isBack = false, location = '') => {
    const {ThongTinNhaKhoaHoc} = this.state;
    if (ThongTinNhaKhoaHoc.LaCanBoTrongTruong) {
      if (ThongTinNhaKhoaHoc.Url !== "" && ThongTinNhaKhoaHoc.Url !== null) {
        const regexUrl = /https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,4}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)/gm;
        const validUrl = regexUrl.test(ThongTinNhaKhoaHoc.Url);
        if (!validUrl) {
          message.destroy();
          message.warning('Đường dẫn Url không hợp lệ');
          return;
        }
      }
    } else {
      ThongTinNhaKhoaHoc.NgaySinh = moment(ThongTinNhaKhoaHoc.NgaySinh).format('YYYY-MM-DD');
      if (ThongTinNhaKhoaHoc.Email !== "" && ThongTinNhaKhoaHoc.Email !== null) {
        const regexEmail = /^[a-z][a-z0-9_.]{0,32}@[a-z0-9]{2,}(\.[a-z0-9]{1,6}){1,3}$/gm;
        const validEmail = regexEmail.test(ThongTinNhaKhoaHoc.Email);
        if (!validEmail) {
          message.destroy();
          message.warning('Email không hợp lệ');
          return;
        }
      }
      if (ThongTinNhaKhoaHoc.Url !== "" && ThongTinNhaKhoaHoc.Url !== null) {
        const regexUrl = /https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,4}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)/gm;
        const validUrl = regexUrl.test(ThongTinNhaKhoaHoc.Url);
        if (!validUrl) {
          message.destroy();
          message.warning('Đường dẫn Url không hợp lệ');
          return;
        }
      }
      if (ThongTinNhaKhoaHoc.TenCanBo === "") {
        message.destroy();
        message.warning('Tên nhà khoa học không được để trống');
        return;
      } else {
        const regexName = /^[a-zA-Z ]{1,50}$/gm; //regex tên tiếng việt
        const validName = regexName.test(this.removeAscent(ThongTinNhaKhoaHoc.TenCanBo));
        if (!validName) {
          message.destroy();
          message.warning('Tên nhà khoa học quá dài hoặc chứa kí tự không hợp lệ');
          return;
        }
      }
      ThongTinNhaKhoaHoc.TenCanBo = this.upperFirstLetter(ThongTinNhaKhoaHoc.TenCanBo);
    }
    api.EditThongTinNhaKhoaHoc(ThongTinNhaKhoaHoc).then(response => {
      if (response.data.Status > 0) {
        this.getInfoNhaKhoaHoc();
        this.setState({saveThongTinCaNhan: false});
        message.destroy();
        message.success('Lưu thông tin cá nhân thành công');
        if (isBack) {
          this.props.history.push(location);
        }
      } else {
        message.destroy();
        message.error(response.data.Message);
      }
    }).catch(error => {
      message.destroy();
      message.error(error.toString());
    })
  };

  saveHuongNghienCuuChinh = (isBack = false, location = '') => {
    const {HuongNghienCuuChinh, ThongTinNhaKhoaHoc} = this.state;
    const data = {
      TenHuongNghienCuuChinh: HuongNghienCuuChinh.TenHuongNghienCuuChinh,
      CanBoID: ThongTinNhaKhoaHoc.CanBoID,
      CoQuanID: ThongTinNhaKhoaHoc.CoQuanID,
      LoaiThongTin: LoaiThongTinNhaKhoaHoc.HuongNghienCuuChinh,
      CTNhaKhoaHocID: HuongNghienCuuChinh.CTNhaKhoaHocID
    };
    const form = new FormData();
    form.append('ThongTinChiTiet', JSON.stringify(data));
    formDataCaller(apiUrl.addeditthongtinchitiet, form).then(response => {
      if (response.data.Status > 0) {
        message.destroy();
        message.success('Cập nhật thông tin thành công');
        this.getInfoNhaKhoaHoc();
        this.setState({saveHuongNghienCuuChinh: false});
        if (isBack) {
          this.props.history.push(location);
        }
      } else {
        message.destroy();
        message.error(response.data.Message);
      }
    }).catch(error => {
      message.destroy();
      message.error(error.toString());
    })
  };

  submitModalQuaTrinhDaoTao = (data) => {
    this.setState({loading: true});
    const form = new FormData();
    form.append('ThongTinChiTiet', JSON.stringify(data));
    formDataCaller(apiUrl.addeditthongtinchitiet, form)
      .then(response => {
        if (response.data.Status > 0) {
          this.getInfoNhaKhoaHoc();
          this.setState({visibleModalQuaTrinhDaoTao: false})
          message.destroy();
          message.success('Cập nhật thông tin thành công');
        } else {
          this.setState({loading: false});
          message.destroy();
          message.error(response.data.Message);
        }
      }).catch(error => {
      this.setState({loading: false});
      message.destroy();
      message.error(error.toString());
    });
  };

  submitModalQuaTrinhCongTac = (data) => {
    this.setState({loading: true});
    const form = new FormData();
    form.append('ThongTinChiTiet', JSON.stringify(data));
    formDataCaller(apiUrl.addeditthongtinchitiet, form)
      .then(response => {
        if (response.data.Status > 0) {
          this.getInfoNhaKhoaHoc();
          this.setState({visibleModalQuaTrinhCongTac: false})
          message.destroy();
          message.success('Cập nhật thông tin thành công');
        } else {
          this.setState({loading: false});
          message.destroy();
          message.error(response.data.Message);
        }
      }).catch(error => {
      this.setState({loading: false});
      message.destroy();
      message.error(error.toString());
    });
  };

  submitModalNgoaiNgu = (data) => {
    this.setState({loading: true});
    const form = new FormData();
    form.append('ThongTinChiTiet', JSON.stringify(data));
    formDataCaller(apiUrl.addeditthongtinchitiet, form)
      .then(response => {
        if (response.data.Status > 0) {
          this.getInfoNhaKhoaHoc();
          this.setState({visibleModalNgoaiNgu: false})
          message.destroy();
          message.success('Cập nhật thông tin thành công');
        } else {
          this.setState({loading: false});
          message.destroy();
          message.error(response.data.Message);
        }
      }).catch(error => {
      this.setState({loading: false});
      message.destroy();
      message.error(error.toString());
    });
  };

  submitModalVanBangChungChi = (data) => {
    this.setState({loading: true});
    const form = new FormData();
    data.NgayCap = data.NgayCap ? moment(data.NgayCap).format("YYYY-MM-DD") : null;
    form.append('ThongTinChiTiet', JSON.stringify(data));
    formDataCaller(apiUrl.addeditthongtinchitiet, form)
      .then(response => {
        if (response.data.Status > 0) {
          this.getInfoNhaKhoaHoc();
          this.setState({visibleModalVanBangChungChi: false})
          message.destroy();
          message.success('Cập nhật thông tin thành công');
        } else {
          this.setState({loading: false});
          message.destroy();
          message.error(response.data.Message);
        }
      }).catch(error => {
      this.setState({loading: false});
      message.destroy();
      message.error(error.toString());
    });
  };

  submitModalGiaiThuongKhoaHoc = (data) => {
    this.setState({loading: true});
    const form = new FormData();
    form.append('ThongTinChiTiet', JSON.stringify(data));
    formDataCaller(apiUrl.addeditthongtinchitiet, form)
      .then(response => {
        if (response.data.Status > 0) {
          this.getInfoNhaKhoaHoc();
          this.setState({visibleModalGiaiThuongKhoaHoc: false})
          message.destroy();
          message.success('Cập nhật thông tin thành công');
        } else {
          this.setState({loading: false});
          message.destroy();
          message.error(response.data.Message);
        }
      }).catch(error => {
      this.setState({loading: false});
      message.destroy();
      message.error(error.toString());
    });
  };

  submitModalDuAnDeTai = (data) => {
    this.setState({loading: true});
    const form = new FormData();
    form.append('ThongTinChiTiet', JSON.stringify(data));
    formDataCaller(apiUrl.addeditthongtinchitiet, form)
      .then(response => {
        if (response.data.Status > 0) {
          this.getInfoNhaKhoaHoc();
          this.setState({visibleModalDuAnDeTai: false})
          message.destroy();
          message.success('Cập nhật thông tin thành công');
        } else {
          this.setState({loading: false});
          message.destroy();
          message.error(response.data.Message);
        }
      }).catch(error => {
      this.setState({loading: false});
      message.destroy();
      message.error(error.toString());
    });
  };

  submitModalBaiBaoTapChi = (data) => {
    this.setState({loading: true});
    const form = new FormData();
    form.append('ThongTinChiTiet', JSON.stringify(data));
    formDataCaller(apiUrl.addeditthongtinchitiet, form)
      .then(response => {
        if (response.data.Status > 0) {
          this.getInfoNhaKhoaHoc();
          this.setState({visibleModalBaiBaoTapChi: false});
          message.destroy();
          message.success('Cập nhật thông tin thành công');
        } else {
          this.setState({loading: false});
          message.destroy();
          message.error(response.data.Message);
        }
      }).catch(error => {
      this.setState({loading: false});
      message.destroy();
      message.error(error.toString());
    });
  };

  submitModalBaoCaoKhoaHoc = (data) => {
    this.setState({loading: true});
    const form = new FormData();
    form.append('ThongTinChiTiet', JSON.stringify(data));
    formDataCaller(apiUrl.addeditthongtinchitiet, form)
      .then(response => {
        if (response.data.Status > 0) {
          this.getInfoNhaKhoaHoc();
          this.setState({visibleModalBaoCaoKhoaHoc: false});
          message.destroy();
          message.success('Cập nhật thông tin thành công');
        } else {
          this.setState({loading: false});
          message.destroy();
          message.error(response.data.Message);
        }
      }).catch(error => {
      this.setState({loading: false});
      message.destroy();
      message.error(error.toString());
    });
  };

  submitModalKetQuaNghienCuu = (data) => {
    this.setState({loading: true});
    const form = new FormData();
    // data.FileDinhKem = [];
    // FileDinhKem.forEach(item => {
    //   if (!item.FileDinhKemID) {
    //     const filedata = {
    //       LoaiFile: LoaiFileDinhKem.KetQuaNghienCuu,
    //       TenFileGoc: item.FileData.name,
    //       FolderPath: 'KetQuaNghienCuu'
    //     };
    //     data.FileDinhKem.push(filedata);
    //     form.append('files', item.FileData)
    //   }
    // });
    form.append('ThongTinChiTiet', JSON.stringify(data));
    formDataCaller(apiUrl.addeditthongtinchitiet, form)
      .then(response => {
        if (response.data.Status > 0) {
          this.getInfoNhaKhoaHoc();
          this.setState({visibleModalKetQuaNghienCuu: false});
          message.destroy();
          message.success('Cập nhật thông tin thành công');
        } else {
          this.setState({loading: false});
          message.destroy();
          message.error(response.data.Message);
        }
      }).catch(error => {
      this.setState({loading: false});
      message.destroy();
      message.error(error.toString());
    });
  };

  submitModalSachChuyenKhao = (data) => {
    this.setState({loading: true});
    const form = new FormData();
    // data.FileDinhKem = [];
    // FileDinhKem.forEach(item => {
    //   if (!item.FileDinhKemID) {
    //     const filedata = {
    //       LoaiFile: LoaiFileDinhKem.SachChuyenKhao,
    //       TenFileGoc: item.FileData.name,
    //       FolderPath: 'SachChuyenKhao'
    //     };
    //     data.FileDinhKem.push(filedata);
    //     form.append('files', item.FileData)
    //   }
    // });
    form.append('ThongTinChiTiet', JSON.stringify(data));
    formDataCaller(apiUrl.addeditthongtinchitiet, form)
      .then(response => {
        if (response.data.Status > 0) {
          this.getInfoNhaKhoaHoc();
          this.setState({visibleModalSachChuyenKhao: false});
          message.destroy();
          message.success('Cập nhật thông tin thành công');
        } else {
          this.setState({loading: false});
          message.destroy();
          message.error(response.data.Message);
        }
      }).catch(error => {
      this.setState({loading: false});
      message.destroy();
      message.error(error.toString());
    });
  };

  submitModalCacMonGiangDay = (data, FileDinhKem) => {
    this.setState({loading: true});
    const form = new FormData();
    data.FileDinhKem = [];
    FileDinhKem.forEach(item => {
      if (!item.FileDinhKemID) {
        const filedata = {
          LoaiFile: LoaiFileDinhKem.CacMonGiangDay,
          TenFileGoc: item.FileData.name,
          FolderPath: 'CacMonGiangDay'
        };
        data.FileDinhKem.push(filedata);
        form.append('files', item.FileData)
      }
    });
    form.append('ThongTinChiTiet', JSON.stringify(data));
    formDataCaller(apiUrl.addeditthongtinchitiet, form)
      .then(response => {
        if (response.data.Status > 0) {
          this.getInfoNhaKhoaHoc();
          this.setState({visibleModalCacMonGiangDay: false})
          message.destroy();
          message.success('Cập nhật thông tin thành công');
        } else {
          this.setState({loading: false});
          message.destroy();
          message.error(response.data.Message);
        }
      }).catch(error => {
      this.setState({loading: false});
      message.destroy();
      message.error(error.toString());
    });
  };

  submitModalHoatDongKhoaHoc = (data, FileDinhKem) => {
    this.setState({loading: true});
    const form = new FormData();
    data.FileDinhKem = [];
    FileDinhKem.forEach(item => {
      if (!item.FileDinhKemID) {
        const filedata = {
          LoaiFile: LoaiFileDinhKem.HoatDongKhoaHoc,
          TenFileGoc: item.FileData.name,
          FolderPath: 'HoatDongKhoaHoc'
        };
        data.FileDinhKem.push(filedata);
        form.append('files', item.FileData)
      }
    });
    form.append('HoatDongKhoaHoc', JSON.stringify(data));
    formDataCaller(apiUrl.addedithoatdongkhoahoc, form)
      .then(response => {
        if (response.data.Status > 0) {
          this.getInfoNhaKhoaHoc();
          this.setState({visibleModalHoatDongKhoaHoc: false})
          message.destroy();
          message.success('Cập nhật thông tin thành công');
        } else {
          this.setState({loading: false});
          message.destroy();
          message.error(response.data.Message);
        }
      }).catch(error => {
      this.setState({loading: false});
      message.destroy();
      message.error(error.toString());
    });
  };

  submitModalSanPhamDaoTao = (data) => {
    this.setState({loading: true});
    const form = new FormData();
    form.append('ThongTinChiTiet', JSON.stringify(data));
    formDataCaller(apiUrl.addeditthongtinchitiet, form)
      .then(response => {
        if (response.data.Status > 0) {
          this.getInfoNhaKhoaHoc();
          this.setState({visibleModalSanPhamDaoTao: false});
          message.destroy();
          message.success('Cập nhật thông tin thành công');
        } else {
          this.setState({loading: false});
          message.destroy();
          message.error(response.data.Message);
        }
      }).catch(error => {
      this.setState({loading: false});
      message.destroy();
      message.error(error.toString());
    });
  };

  getBase64 = (file, callback) => {
    const reader = new FileReader();
    reader.addEventListener('load', () => callback(reader.result, file));
    reader.readAsDataURL(file);
  };

  beforeUploadAvatar = (file) => {
    const {FileLimit} = this.props;
    const isLt2M = file.size / 1024 / 1024 < FileLimit;
    const isJpgOrPng = file.type === 'image/jpeg' || file.type === 'image/png';
    if (!isLt2M) {
      message.error(`File đính kèm phải nhỏ hơn ${FileLimit}MB`);
    } else if (!isJpgOrPng) {
      message.error('Chỉ được tải file có đuôi .jpg hoặc .png');
    } else {
      this.genDataAvatar(file);
    }
    return false;
  };

  genDataAvatar = (file) => {
    const {ThongTinNhaKhoaHoc} = this.state;
    const CanBoID = ThongTinNhaKhoaHoc.CanBoID;
    const CoQuanID = ThongTinNhaKhoaHoc.CoQuanID;
    const NoiDung = {
      NghiepVuID: CanBoID,
      CoQuanID: CoQuanID
    };
    const form = new FormData();
    form.append('AnhDaiDien', file);
    form.append('NoiDung', JSON.stringify(NoiDung));
    formDataCaller(apiUrl.capnhatanhdaidien, form)
      .then(response => {
        if (response.data.Status > 0) {
          message.destroy();
          message.success('Cập nhật ảnh đại diện thành công');
          this.getInfoNhaKhoaHoc();
        } else {
          message.destroy();
          message.error(response.data.Message);
        }
      }).catch(error => {
      message.destroy();
      message.error(error.toString());
    });
  };

  beforeUploadFileLyLich = (file) => {
    const {FileLimit, FileAllow} = this.props;
    const isLt2M = file.size / 1024 / 1024 < FileLimit;
    if (!isLt2M) {
      message.error(`File đính kèm phải nhỏ hơn ${FileLimit}MB`);
    } else if (!checkValidFileName(file.name, FileAllow)) {
      message.error(`File không hợp lệ`);
    } else {
      this.genFileGioiThieu(file);
    }
    return false;
  };

  genFileGioiThieu = (file) => {
    const {ThongTinNhaKhoaHoc} = this.state;
    const CanBoID = ThongTinNhaKhoaHoc.CanBoID;
    const NoiDung = {
      NghiepVuID: CanBoID,
      LoaiFile: LoaiFileDinhKem.LyLich,
      FolderPath: 'LyLich'
    };
    const form = new FormData();
    form.append('files', file);
    form.append('NoiDung', JSON.stringify(NoiDung));
    formDataCaller(apiUrl.themmoifiledinhkem, form)
      .then(response => {
        if (response.data.Status > 0) {
          message.destroy();
          message.success('Thêm mới file lý lịch thành công');
          this.getInfoNhaKhoaHoc();
        } else {
          message.destroy();
          message.error(response.data.Message);
        }
      }).catch(error => {
      message.destroy();
      message.error(error.toString());
    });
  };

  deleteFileGioiThieu = (FileDinhKemID) => {
    Modal.confirm({
      title: 'Thông báo',
      content: 'Xác nhận xóa file giới thiệu ?',
      okText: 'Có',
      cancelText: 'Không',
      onOk: () => {
        const FileDinhKemModel = {FileDinhKemID: FileDinhKemID};
        api.XoaFileDinhKem(FileDinhKemModel).then(response => {
          if (response.data.Status > 0) {
            message.destroy();
            message.success('Xóa file lý lịch thành công');
            this.getInfoNhaKhoaHoc();
          } else {
            message.destroy();
            message.error(response.data.Message);
          }
        }).catch(error => {
          message.destroy();
          message.error(error.toString());
        });
      }
    });
  };

  renderFileGioiThieu = (ListFileGioiThieu) => {
    const {disableEdit} = this.state;
    if (ListFileGioiThieu && ListFileGioiThieu.length > 0) {
      return ListFileGioiThieu.map(item => (
        <Row>
          <Col span={16}>
            <a href={item.FileUrl} className={'wrap-text link-hover'} target={"_blank"}>
              {item.TenFileGoc}
            </a>
          </Col>
          <Col span={4}/>
          <Col span={4} style={{textAlign: 'center'}}>
            {disableEdit ? "" :
              <Icon type={'delete'} style={{color: 'red'}}
                    onClick={() => this.deleteFileGioiThieu(item.FileDinhKemID)}/>}
          </Col>
        </Row>
      ))
    }
  };

  renderTenChucVu = (ListID) => {
    if (!ListID) {
      return "";
    }
    const {DanhSachHocVi} = this.props;
    let DanhSachTenChucVu = [];
    ListID.forEach(Id => {
      const chucvu = DanhSachHocVi.filter(item => item.Id === Id);
      if (chucvu && chucvu.length > 0) {
        DanhSachTenChucVu.push(chucvu[0].Name);
      }
    });
    return DanhSachTenChucVu.join(", ");
  };

  renderTenChucDanh = (ListID) => {
    if (!ListID) {
      return "";
    }
    const {DanhSachChucDanh} = this.props;
    let DanhSachTenChucDanh = [];
    ListID.forEach(Id => {
      const chucdanh = DanhSachChucDanh.filter(item => item.Id === Id);
      if (chucdanh && chucdanh.length > 0) {
        DanhSachTenChucDanh.push(chucdanh[0].Name);
      }
    });
    return DanhSachTenChucDanh.join(", ");
  };

  renderTenPhongBan = (ID) => {
    if (!ID) {
      return "";
    }
    const {DanhSachPhongBan} = this.props;
    const phongban = DanhSachPhongBan.filter(item => item.Id == ID);
    return phongban && phongban.length > 0 ? phongban[0].Name : "";
  };

  renderTenTacGia = (ListID) => {
    const {DanhSachCanBo} = this.props;
    let ListTenTacGia = [];
    if (ListID && ListID.length) {
      ListID.forEach(tg => {
        const tacgia = DanhSachCanBo.find(item => item.CanBoID === tg.CanBoID && item.CoQuanID === tg.CoQuanID);

        ListTenTacGia.push(tacgia ? tacgia.TenCanBo : "");
      })
    }
    ListTenTacGia = ListTenTacGia.filter(item => item !== "");
    return ListTenTacGia.length > 0 ? ListTenTacGia.join(", ") : "";
  };

  renderContentPrint = () => {
    //↵
    const {ThongTinNhaKhoaHoc} = this.state;
    const {DanhSachCanBo} = this.props;
    const {QuaTrinhDaoTao, QuaTrinhCongTac, NgoaiNgu, HuongNghienCuuChinh, DuAnDeTai, BaiBaoTapChi, KetQuaNghienCuu, SachChuyenKhao, VanBangChungChi, GiaiThuongKhoaHoc, SanPhamDaoTao, CacMonGiangDay, HoatDongKhoaHoc} = this.state;
    const style = {
      table: {
        width: '100%',
        borderCollapse: 'collapse'
      },
      td: {
        border: 'solid 1px',
        padding: 5
      }
    };
    const BaiBaoFormat = this.formatDanhSachBaiBao();

    return (
      <div style={{fontFamiLy: 'Times New Roman'}}>
        <table style={{width: '100%'}}>
          <tr>
            <td style={{width: '80%'}}>
              <img src={logoHNUEBase64()} style={{width: 100, height: 100}} alt={''}/>
            </td>
            <td style={{width: '20%', textAlign: 'center'}}/>
          </tr>
          <tr/>
          <tr>
            <td style={{textAlign: 'center'}} colSpan={2}>
              <b>LÝ LỊCH KHOA HỌC</b>
            </td>
          </tr>
        </table>
        <br/>
        <div>
          <b style={{fontSize: 16}}>1. Thông tin cá nhân</b>
          <table style={{border: 'solid 1px #fff', borderCollapse: 'collapse'}}>
            <tr>
              <td rowSpan={2} style={style.td}>1</td>
              <td style={style.td}>Họ và tên</td>
              <td colSpan={2}
                  style={{textTransform: 'uppercase', ...style.td}}>{ThongTinNhaKhoaHoc.TenCanBo}</td>
              <td style={{...style.td}}>Ngày sinh</td>
              <td
                style={style.td}>{ThongTinNhaKhoaHoc.NgaySinh ? moment(ThongTinNhaKhoaHoc.NgaySinh).format('DD/MM/YYYY') : ""}</td>
              <td style={style.td}>{ThongTinNhaKhoaHoc.GioiTinh ? "Nam" : "Nữ"}</td>
            </tr>
            <tr>
              <td style={style.td} colSpan={2}>Học hàm, học vị</td>
              <td style={style.td}>
                {this.renderTenChucVu(ThongTinNhaKhoaHoc.ChucDanhKhoaHoc)}
              </td>
              <td style={style.td} colSpan={2}>Chức vụ hành chính</td>
              <td style={style.td}>
                {this.renderTenChucDanh(ThongTinNhaKhoaHoc.ChucDanhHanhChinh)}
              </td>
            </tr>
            <tr>
              <td rowSpan={2} style={style.td}>2</td>
              <td colSpan={6} style={style.td}>
                Cơ quan công tác và địa chỉ:&ensp;
                {ThongTinNhaKhoaHoc.CoQuanCongTac}
                {ThongTinNhaKhoaHoc.DiaChiCoQuan && ThongTinNhaKhoaHoc.DiaChiCoQuan !== "" ? ", " : ""}
                {ThongTinNhaKhoaHoc.DiaChiCoQuan}
              </td>
            </tr>
            <tr>
              <td colSpan={3} style={style.td}>Khoa/ Phòng ban</td>
              <td colSpan={3} style={style.td}>
                {this.renderTenPhongBan(ThongTinNhaKhoaHoc.PhongBanID)}
              </td>
            </tr>
            <tr>
              <td rowSpan={2} style={style.td}>3</td>
              <td style={style.td}>Điện thoại</td>
              <td colSpan={2} style={style.td}>{ThongTinNhaKhoaHoc.DienThoai}</td>
              <td style={style.td}>Điện thoai di động</td>
              <td colSpan={2} style={style.td}>{ThongTinNhaKhoaHoc.DienThoaiDiDong}</td>
            </tr>
            <tr>
              <td style={style.td}>Fax</td>
              <td colSpan={2} style={style.td}>{ThongTinNhaKhoaHoc.Fax}</td>
              <td style={style.td}>E-mail <i>(đã đăng kí trên hệ thống OMS, nếu có)</i></td>
              <td colSpan={2} style={style.td}>{ThongTinNhaKhoaHoc.Email}</td>
            </tr>
            <tr>
              <td style={{width: '4%', border: '#000'}}/>
              <td style={{width: '16%', border: '#000'}}/>
              <td style={{width: '16%', border: '#000'}}/>
              <td style={{width: '16%', border: '#000'}}/>
              <td style={{width: '16%', border: '#000'}}/>
              <td style={{width: '16%', border: '#000'}}/>
              <td style={{width: '16%', border: '#000'}}/>
            </tr>
          </table>
        </div>
        <br/>
        <div>
          <b style={{fontSize: 16}}>2. Quá trình đào tạo</b>
          <table style={{borderCollapse: 'collapse', width: '100%'}}>
            <tr>
              <th style={{...style.td, textAlign: 'center', width: '15%'}}>Thời gian</th>
              <th style={{...style.td, textAlign: 'center', width: '35%'}}>Tên cơ sở đào tạo</th>
              <th style={{...style.td, textAlign: 'center', width: '25%'}}>Chuyên ngành</th>
              <th style={{...style.td, textAlign: 'center', width: '25%'}}>Học vị</th>
            </tr>
            {QuaTrinhDaoTao.map(item => (
              <tr>
                <td style={{...style.td, textAlign: 'center'}}>{item.KhoangThoiGian}</td>
                <td style={style.td}>{item.CoSoDaoTao}</td>
                <td style={style.td}>{item.ChuyenNganh}</td>
                <td style={style.td}>{item.HocVi}</td>
              </tr>
            ))}
          </table>
        </div>
        <br/>
        <div>
          <b style={{fontSize: 16}}>3. Quá trình công tác</b>
          <table style={{borderCollapse: 'collapse', width: '100%'}}>
            <tr>
              <th style={{...style.td, textAlign: 'center', width: '15%'}}>Thời gian</th>
              <th style={{...style.td, textAlign: 'center', width: '35%'}}>Cơ quan công tác</th>
              <th style={{...style.td, textAlign: 'center', width: '25%'}}>Địa chỉ</th>
              <th style={{...style.td, textAlign: 'center', width: '25%'}}>Vị trí công tác</th>
            </tr>
            {QuaTrinhCongTac.map(item => (
              <tr>
                <td style={{...style.td, textAlign: 'center'}}>{item.KhoangThoiGian}</td>
                <td style={style.td}>{item.CoQuanCongTac}</td>
                <td style={style.td}>{item.DiaChiDienThoai}</td>
                <td style={style.td}>{item.ChucVu}</td>
              </tr>
            ))}
          </table>
        </div>
        <br/>
        <div>
          <b style={{fontSize: 16}}>4. Ngoại ngữ</b> (Theo các mức: A- Yếu; B- Trung bình; C- Khá; D- Thành thạo)
          <table style={{borderCollapse: 'collapse', width: '100%'}}>
            <tr>
              <th style={{...style.td, textAlign: 'center'}}>Ngoại ngữ</th>
              <th style={{...style.td, textAlign: 'center'}}>Đọc</th>
              <th style={{...style.td, textAlign: 'center'}}>Viết</th>
              <th style={{...style.td, textAlign: 'center'}}>Nói</th>
            </tr>
            {NgoaiNgu.map(item => (
              <tr>
                <td style={{...style.td, textAlign: 'center'}}>{item.TenNgoaiNgu}</td>
                <td style={{...style.td, textAlign: 'center'}}>{item.Doc}</td>
                <td style={{...style.td, textAlign: 'center'}}>{item.Viet}</td>
                <td style={{...style.td, textAlign: 'center'}}>{item.Noi}</td>
              </tr>
            ))}
          </table>
        </div>
        <br/>
        <div>
          <b style={{fontSize: 16}}>5. Văn bằng chứng chỉ</b>
          {VanBangChungChi.map(item => <div>
            &ensp;- {[item.NgayCap ? moment(item.NgayCap).format('DD/MM/YYYY') : "", [item.TieuDe, item.TrinhDo !== "" ? `(${item.TrinhDo})` : ""].filter(item => item !== "").join(" "), item.NoiCap, item.SoHieu].filter(item => item !== "").join(", ")}
          </div>)}
        </div>
        <br/>
        <div>
          <b style={{fontSize: 16}}>6. Khen thưởng</b>
          <table style={{borderCollapse: 'collapse', width: '100%'}}>
            <tr>
              <th style={{...style.td, textAlign: 'center', width: '10%'}}>STT</th>
              <th style={{...style.td, textAlign: 'center', width: '50%'}}>Hình thức và nội dung giải thưởng</th>
              <th style={{...style.td, textAlign: 'center', width: '40%'}}>Năm tặng thưởng</th>
            </tr>
            {GiaiThuongKhoaHoc.map((item, index) => (
              <tr>
                <td style={{...style.td, textAlign: 'center'}}>{index + 1}</td>
                <td style={{...style.td}}>{item.TieuDe}</td>
                <td style={{...style.td}}>{item.KhoangThoiGian}</td>
              </tr>
            ))}
          </table>
        </div>
        <br/>
        <div>
          <b style={{fontSize: 16}}>7. Kinh nghiệm và thành tích nghiên cứu</b>
          <br/>
          &ensp; 7.1. Hướng nghiên cứu chính theo đuổi trong 5 năm gần đây.
          {HuongNghienCuuChinh.TenHuongNghienCuuChinh ? HuongNghienCuuChinh.TenHuongNghienCuuChinh.split("\n").map(item => (
            <div>&emsp;&ensp;{item}</div>
          )) : ""}
          <br/>
          &ensp; 7.2. Danh sách đề tài/dự án nghiên cứu tham gia thực hiện hoặc đã nộp hồ sơ trong 5 năm gần nhất:
          <table style={{borderCollapse: 'collapse', width: '100%'}}>
            <tr>
              <th style={{...style.td, textAlign: 'center', width: '5%'}}>STT</th>
              <th style={{...style.td, textAlign: 'center', width: '25%'}}>Tên đề tài/Dự án</th>
              <th style={{...style.td, textAlign: 'center', width: '20%'}}>Cơ quan tài trợ kinh phí</th>
              <th style={{...style.td, textAlign: 'center', width: '20%'}}>Thời gian thực hiện</th>
              <th style={{...style.td, textAlign: 'center', width: '20%'}}>Vai trò tham gia</th>
            </tr>
            {DuAnDeTai.map((item, index) => (
              <tr>
                <td style={{...style.td, textAlign: 'center'}}>{index + 1}</td>
                <td style={{...style.td, textAlign: 'left'}}>{item.TenDuAn}</td>
                <td style={{...style.td, textAlign: 'left'}}>{item.CoQuanTaiTro}</td>
                <td style={{...style.td, textAlign: 'left'}}>{item.KhoangThoiGian}</td>
                <td style={{...style.td, textAlign: 'left'}}>{item.VaiTroThamGia}</td>
              </tr>
            ))}
          </table>
          <br/>
          &ensp; 7.3. Kết quả nghiên cứu đã được công bố hoặc đăng ký trong 5 năm gần nhất.
          <br/>
          &emsp; 7.3.1 Các bài báo trên tạp chí khoa học quốc tế
          <br/>
          &emsp; &ensp;• Thuộc danh mục SCI, SCIE, SSCI, AHCI (ISI)
          <br/>
          {BaiBaoFormat[0].map(item => <div>&emsp;&emsp;- {renderContentBaiBao(item, DanhSachCanBo)}</div>)}
          <br/>
          &emsp; &ensp;• Thuộc danh mục SCOPUS
          <br/>
          {BaiBaoFormat[1].map(item => <div>&emsp;&emsp;- {renderContentBaiBao(item, DanhSachCanBo)}</div>)}
          <br/>
          &emsp; &ensp;• Bài báo quốc tế khác
          <br/>
          {BaiBaoFormat[2].map(item => <div>&emsp;&emsp;- {renderContentBaiBao(item, DanhSachCanBo)}</div>)}
          <br/>
          &emsp; 7.3.2 Các bài báo trên tạp chí khoa học trong nước
          <br/>
          {BaiBaoTapChi.filter(item => item.LoaiBaiBao === 2).map(item =>
            <div>&emsp;&emsp;- {renderContentBaiBao(item, DanhSachCanBo)}</div>)}
          <br/>
          &emsp; 7.3.3 Các báo cáo khoa học trong hội thảo quốc tế, trong nước
          <br/>
          {BaiBaoTapChi.filter(item => item.LoaiBaiBao > 2).map(item =>
            <div>&emsp;&emsp;- {renderContentBaiBao(item, DanhSachCanBo)}</div>)}
          <br/>
          &emsp; 7.3.4 Các sách đã xuất bản (chuyên khảo, giáo trình, tham khảo,…)
          <br/>
          {SachChuyenKhao.map(item => <div>&emsp;&emsp;- {renderContentSachChuyenKhao(item, DanhSachCanBo)}</div>)}
          <br/>
          &emsp; 7.3.5 Các kết quả nghiên cứu khác được công bố (bằng phát minh, sáng chế/ giải pháp hữu ích,…)
          <br/>
          {/*{KetQuaNghienCuu.map(item => <div>&emsp;&emsp;- {renderContentKetQuaNghienCuu(item, DanhSachCanBo)}</div>)}*/}
          <table style={{borderCollapse: 'collapse', width: '100%'}}>
            <tr>
              <th style={{...style.td, textAlign: 'center', width: '10%'}}>STT</th>
              <th style={{...style.td, textAlign: 'center', width: '25%'}}>Tên công trình/ văn bằng</th>
              <th style={{...style.td, textAlign: 'center', width: '20%'}}>Thời gian</th>
              <th style={{...style.td, textAlign: 'center', width: '25%'}}>Tác giả</th>
              <th style={{...style.td, textAlign: 'center', width: '20%'}}>Ghi chú</th>
            </tr>
            {KetQuaNghienCuu.map((item, index) =>
              <tr>
                <td style={{...style.td, textAlign: 'center'}}>{index + 1}</td>
                <td style={{...style.td}}>{item.TieuDe}</td>
                <td style={{...style.td, textAlign: 'center'}}>{item.NamXuatBan}</td>
                <td style={{...style.td}}>{this.renderTenTacGia(item.ListTacGia)}</td>
                <td style={{...style.td}}>{item.GhiChu}</td>
              </tr>
            )}
          </table>
        </div>
        <br/>
        <div>
          <b style={{fontSize: 16}}>8. Kết quả đào tạo</b>
          <br/>
          &ensp; 8.1. Các môn giảng dạy
          {CacMonGiangDay.map(item => <div>
            &ensp;- {item.TieuDe}{item.DeCuong !== "" ? ` (${item.DeCuong}) ` : ""}
          </div>)}
          <br/>
          &ensp; 8.2. Kết quả hướng dẫn
          <table style={{borderCollapse: 'collapse', width: '100%'}}>
            <tr>
              <th rowSpan={2} style={{...style.td, textAlign: 'center', width: '5%'}}>STT</th>
              <th rowSpan={2} style={{...style.td, textAlign: 'center', width: '15%'}}>Họ tên</th>
              <th colSpan={3} style={{...style.td, textAlign: 'center'}}>Bậc học</th>
              <th colSpan={2} style={{...style.td, textAlign: 'center'}}>Trách nhiệm</th>
              <th rowSpan={2} style={{...style.td, textAlign: 'center', width: '15%'}}>Tên khóa luận/ luận văn/ luận
                án
              </th>
              <th rowSpan={2} style={{...style.td, textAlign: 'center', width: '15%'}}>Thời gian hướng dẫn</th>
              <th rowSpan={2} style={{...style.td, textAlign: 'center', width: '15%'}}>Cơ sở đào tạo</th>
              <th rowSpan={2} style={{...style.td, textAlign: 'center', width: '10%'}}>Năm bảo vệ</th>
            </tr>
            <tr>
              <td style={{...style.td, textAlign: 'center', width: '5%'}}>Cử nhân</td>
              <td style={{...style.td, textAlign: 'center', width: '5%'}}>Thạc sỹ</td>
              <td style={{...style.td, textAlign: 'center', width: '5%'}}>Tiến sỹ</td>
              <td style={{...style.td, textAlign: 'center', width: '5%'}}>Chính</td>
              <td style={{...style.td, textAlign: 'center', width: '5%'}}>Phụ</td>
            </tr>
            {SanPhamDaoTao.map((item, index) => <tr>
              <td style={{...style.td, textAlign: 'center'}}>{index + 1}</td>
              <td style={{...style.td,}}>{item.TenHocVien}</td>
              <td style={{...style.td, textAlign: 'center'}}>{item.LoaiDaoTao === 1 ? "✓" : ""}</td>
              <td style={{...style.td, textAlign: 'center'}}>{item.LoaiDaoTao === 2 ? "✓" : ""}</td>
              <td style={{...style.td, textAlign: 'center'}}>{item.LoaiDaoTao === 3 ? "✓" : ""}</td>
              <td style={{...style.td, textAlign: 'center'}}>{item.CapHoiThao === 1 ? "✓" : ""}</td>
              <td style={{...style.td, textAlign: 'center'}}>{item.CapHoiThao === 2 ? "✓" : ""}</td>
              <td style={{...style.td,}}>{item.TenLuanVan}</td>
              <td style={{...style.td,}}>{item.KhoangThoiGian}</td>
              <td style={{...style.td,}}>{item.CoSoDaoTao}</td>
              <td style={{...style.td, textAlign: 'center'}}>{item.NamBaoVe}</td>
            </tr>)}
          </table>
        </div>
        <br/>
        <div>
          <b style={{fontSize: 16}}>9. Hoạt động khoa học khác</b>
          <br/>
          <table style={{borderCollapse: 'collapse', width: '100%'}}>
            <tr>
              <th style={{...style.td, textAlign: 'center', width: '10%'}}>STT</th>
              <th style={{...style.td, textAlign: 'center', width: '80%'}}>Hoạt động khoa học</th>
              <th style={{...style.td, textAlign: 'center', width: '10%'}}>Năm thực hiện</th>
            </tr>
            {HoatDongKhoaHoc.map((item, index) => <tr>
              <td style={{...style.td, textAlign: 'center'}}>{index + 1}</td>
              <td style={{...style.td}}>{item.HoatDongKhoaHoc}</td>
              <td style={{...style.td, textAlign: 'center'}}>{item.NamThucHien}</td>
            </tr>)}
          </table>
        </div>
        <br/>
        <div>
          Tôi xin cam kết và chịu trách nhiệm về tính chính xác của các thông tin cung cấp trong lý lịch khoa học này
          <br/>
          <br/>
          <table style={{width: '100%'}}>
            <td style={{width: '50%', textAlign: 'center'}}>
              <b style={{fontSize: 16}}>XÁC NHẬN CỦA CƠ QUAN CÔNG TÁC</b>
              <br/>
              <i>(Ký, ghi rõ họ tên và đóng dấu)</i>
            </td>
            <td style={{width: '50%', textAlign: 'center'}}>
              . . . . . . ngày . . . . tháng . . . . năm . . . .
              <br/>
              <b style={{fontSize: 16}}>NGƯỜI KHAI LÝ LỊCH</b>
              <br/>
              <i>(Ký, ghi rõ họ tên)</i>
            </td>
          </table>
        </div>
      </div>
    )
  };

  printWord = () => {
    let html, link, blob, url;
    let preHtml = "<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:w='urn:schemas-microsoft-com:office:word' xmlns='http://www.w3.org/TR/REC-html40'><head><meta charset='utf-8'><title>Export HTML To Doc</title></head><body>";
    let postHtml = "</body></html>";
    html = preHtml + this.formPrint.current.innerHTML + postHtml;
    blob = new Blob(['\ufeff', html], {
      type: 'application/msword'
    });
    url = 'data:application/vnd.ms-word;charset=utf-8,' + encodeURIComponent(html);
    link = document.createElement('A');
    link.href = url;
    link.download = 'Lý lịch nhà khoa học.doc';  // default name without extension
    document.body.appendChild(link);
    if (navigator.msSaveOrOpenBlob) navigator.msSaveOrOpenBlob(blob, 'Lý lịch nhà khoa học.doc'); // IE10-11
    else link.click();  // other browsers
    document.body.removeChild(link);
  };

  deleteNhaKhoaHoc = () => {
    const {ThongTinNhaKhoaHoc, filterData} = this.state;
    const CanBoID = ThongTinNhaKhoaHoc.CanBoID;
    Modal.confirm({
      title: 'Xác nhận xóa',
      content: 'Bạn có muốn xóa thông tin nhà khoa học này không ?',
      okText: 'Có',
      cancelText: 'Không',
      onOk: () => {
        api.DeleteNhaKhoaHoc({CanBoID: CanBoID}).then(response => {
          if (response.data.Status > 0) {
            message.destroy();
            message.success('Xóa nhà khoa học thành công');
            if (filterData.ref) {
              this.props.history.push(filterData.ref)
            } else {
              this.getInfoNhaKhoaHoc();
            }
          } else {
            message.destroy();
            message.error(response.data.Message);
          }
        }).catch(error => {
          message.destroy();
          message.error(error.toString())
        });
      }
    });
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

  inputNumber = (e) => {
    const key = e.charCode;
    if (key < 48 || key > 57) {
      e.preventDefault();
    }
  };

  removeAscent = (str) => {
    if (str === null || str === undefined) return str;
    str = str.toLowerCase();
    str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
    str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
    str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
    str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
    str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
    str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
    str = str.replace(/đ/g, "d");
    return str;
  };

  validUrl = (url) => {
    const regexUrl = /https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,4}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)/gm;
    const validUrl = regexUrl.test(url);
    if (!validUrl) {
      message.destroy();
      return false;
    }
    return true
  };

  clickUrl = (url) => {
    if (this.validUrl(url)) {
      window.open(url);
    }
  };

  handleBlockedNavigation = (location) => {
    const {saveThongTinCaNhan, saveHuongNghienCuuChinh} = this.state;
    if ((saveThongTinCaNhan || saveHuongNghienCuuChinh) && !this.isBackButtonClicked && location.pathname !== "/") {
      Modal.confirm({
        content: 'Bạn có muốn lưu dữ liệu thay đổi trước khi thoát ?',
        onOk: () => {
          if (saveHuongNghienCuuChinh && saveThongTinCaNhan) {
            this.saveThongTinCaNhan();
            this.saveHuongNghienCuuChinh(true, location.pathname);
          } else if (saveThongTinCaNhan) {
            this.saveThongTinCaNhan(true, location.pathname);
          } else if (saveHuongNghienCuuChinh) {
            this.saveHuongNghienCuuChinh(true, location.pathname);
          }
        },
        onCancel: () => {
          this.isBackButtonClicked = true;
          this.props.history.push(location.pathname)
        },
        okText: 'Có',
        cancelText: 'Không'
      });
      return false;
    }
    return true;
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

  printPDF = () => {
    //xoa iframe cu ------
    let oldIframe = document.querySelectorAll("iframe");
    if (oldIframe && oldIframe.length) {
      oldIframe.forEach((element) => {
        element.parentNode.removeChild(element);
      });
    }
    //tao iframe moi -----
    let node = this.formPrint.current.innerHTML;
    let iframe = document.createElement("iframe");
    iframe.style.display = "none";
    document.body.appendChild(iframe); //make document #html in iframe
    iframe.contentWindow.document.open();
    iframe.contentWindow.document.write(node);
    iframe.contentWindow.document.close();
    iframe.contentWindow.focus();
    iframe.contentWindow.print();
  };

  changeNguoiGioiThieu = (value) => {
    if (value && value.length > 0) {
      const NguoiGioiThieu = [];
      value.forEach(item => {
        const arr = item.split("_");
        const NguoiGioiThieuID = arr[0];
        const CoQuanGioiThieuID = arr[1];
        const data = {
          NguoiGioiThieuID: NguoiGioiThieuID,
          CoQuanGioiThieuID: CoQuanGioiThieuID
        };
        NguoiGioiThieu.push(data);
      });
      this.changeInfoNhaKhoaHoc(NguoiGioiThieu, 'NguoiGioiThieu');
    } else {
      this.changeInfoNhaKhoaHoc(value, 'NguoiGioiThieu');
    }
  };

  getLinkGioiThieu = () => {
    const {ThongTinNhaKhoaHoc, filterData} = this.state;
    const {DanhSachCanBo} = this.props;
    const {NguoiGioiThieu} = ThongTinNhaKhoaHoc;
    if (NguoiGioiThieu && NguoiGioiThieu.length > 0) {
      const link = window.location.href;
      const arr = link.split('?');
      const root = arr[0];
      return NguoiGioiThieu.map(item => {
        const param = `?CanBoID=${item.NguoiGioiThieuID}&CoQuanID=${item.CoQuanGioiThieuID}`;
        const href = root + param;
        const TenCanBo = DanhSachCanBo.filter(canbo => canbo.CanBoID == item.NguoiGioiThieuID && canbo.CoQuanID == item.CoQuanGioiThieuID);
        if (TenCanBo[0]) {
          return <div>
            <a target={'_blank'} href={href}>{TenCanBo[0].TenCanBo}</a>
          </div>
        }
      });
    }
    return null;
  };

  getValueNguoiGioiThieu = (NguoiGioiThieu) => {
    return NguoiGioiThieu ? NguoiGioiThieu.map(item => `${item.NguoiGioiThieuID}_${item.CoQuanGioiThieuID}`) : [];
  };

  formatDanhSachBaiBao = () => {
    const {BaiBaoTapChi} = this.state;
    const {DanhSachLoaiKetQua} = this.props;
    const ListLoai1 = [], ListLoai2 = [], ListLoai3 = [], ListLoai4 = [], LoaiKetQua1 = [], LoaiKetQua2 = [],
      LoaiKetQua3 = [];
    if (DanhSachLoaiKetQua.length) {
      DanhSachLoaiKetQua[0].Children[0].Children[0].Children.forEach(item => LoaiKetQua1.push(item.MappingId));
      DanhSachLoaiKetQua[0].Children[0].Children[1].Children.forEach(item => LoaiKetQua2.push(item.MappingId));
      DanhSachLoaiKetQua[0].Children[0].Children[2].Children.forEach(item => LoaiKetQua3.push(item.MappingId));
    }
    BaiBaoTapChi.filter(item => item.LoaiBaiBao === 1).forEach(item => {
      if (item.NhiemVu) {
        if (LoaiKetQua1.includes(item.NhiemVu)) {
          ListLoai1.push(item);
        } else if (LoaiKetQua2.includes(item.NhiemVu)) {
          ListLoai2.push(item);
        } else if (LoaiKetQua3.includes(item.NhiemVu)) {
          ListLoai3.push(item);
        } else {
          ListLoai4.push(item);
        }
      } else {
        ListLoai4.push(item);
      }
    });
    if (ListLoai4.length) {
      ListLoai4.forEach(item => {
        if (item.ChiSo) {
          if (item.ChiSo === 1) {
            ListLoai1.push(item);
          } else if (item.ChiSo === 2) {
            ListLoai2.push(item)
          } else if (item.ChiSo === 3) {
            ListLoai3.push(item)
          }
        }
      })
    }
    return [ListLoai1, ListLoai2, ListLoai3];
  };

  inputTen = (e) => {
    const key = e.charCode;
    if (
      (key === 32 && e.target.value[e.target.value.length - 1] === " ") ||
      (key === 32 && e.target.value.length === 0)
    ) {
      e.preventDefault();
    }
  };

  render() {
    const {modalKey, loading, filterData, actions, disableEdit, disableAdd, disableDelete, loadingData, CanBoTrongTruong, errAPI} = this.state;
    const {saveThongTinCaNhan, ThongTinNhaKhoaHoc} = this.state;
    const {AnhHoSo, NguoiGioiThieu} = ThongTinNhaKhoaHoc;
    const {QuaTrinhDaoTao, visibleModalQuaTrinhDaoTao, dataEditQuaTrinhDaoTao} = this.state;
    const {QuaTrinhCongTac, visibleModalQuaTrinhCongTac, dataEditQuaTrinhCongTac} = this.state;
    const {NgoaiNgu, visibleModalNgoaiNgu, dataEditNgoaiNgu} = this.state;
    const {VanBangChungChi, visibleModalVanBangChungChi, dataEditVanBangChungChi} = this.state;
    const {GiaiThuongKhoaHoc, visibleModalGiaiThuongKhoaHoc, dataEditGiaiThuongKhoaHoc} = this.state;
    const {saveHuongNghienCuuChinh, HuongNghienCuuChinh} = this.state;
    const {DuAnDeTai, visibleModalDuAnDeTai, dataEditDuAnDeTai} = this.state;
    const {BaiBaoTapChi, visibleModalBaiBaoTapChi, dataEditBaiBaoTapChi, LoaiBaiBao} = this.state;
    const {BaoCaoKhoaHoc, visibleModalBaoCaoKhoaHoc, dataEditBaoCaoKhoaHoc} = this.state;
    const {KetQuaNghienCuu, visibleModalKetQuaNghienCuu, dataEditKetQuaNghienCuu} = this.state;
    const {SachChuyenKhao, visibleModalSachChuyenKhao, dataEditSachChuyenKhao} = this.state;
    const {CacMonGiangDay, visibleModalCacMonGiangDay, dataEditCacMonGiangDay} = this.state;
    const {HoatDongKhoaHoc, visibleModalHoatDongKhoaHoc, dataEditHoatDongKhoaHoc} = this.state;
    const {SanPhamDaoTao, visibleModalSanPhamDaoTao, dataEditSanPhamDaoTao} = this.state;
    const {TableLoading, role, FileLimit, roleQL, FileAllow} = this.props;
    const {DanhSachChucDanh, DanhSachHocVi, DanhSachPhongBan, DanhSachNhiemVuKhoaHoc, DanhSachDeTai, DanhSachLoaiNhiemVu, DanhSachCanBo} = this.props;
    let ref = filterData.ref;
    const GioiTinh = ThongTinNhaKhoaHoc.GioiTinh !== null ? ThongTinNhaKhoaHoc.GioiTinh : ThongTinNhaKhoaHoc.GioiTinhStr !== "Nam" ? 0 : 1;
    return (
      <LayoutWrapper>
        <Prompt
          when={saveHuongNghienCuuChinh || saveThongTinCaNhan}
          message={(location) => this.handleBlockedNavigation(location)}
        />
        {TableLoading || loadingData ? <div className={'loading-div'}>
          <Spin/>
        </div> : ""}
        <PageHeader>Thông tin lý lịch nhà khoa học</PageHeader>
        <PageAction>
          {ref ? <Link to={`${ref}`}><Button>Quay lại</Button></Link> : ""}
          {roleQL.delete && !ThongTinNhaKhoaHoc.LaCanBoTrongTruong ?
            <Button type={'primary'} onClick={this.deleteNhaKhoaHoc}>Xóa thông tin</Button> : ""}
          {/*{role.view ? <Button type={'primary'} onClick={() => this.printWord()}>Xuất lý lịch</Button> : ""}*/}
          <Button type={'primary'} onClick={() => this.printWord()}>Xuất lý lịch</Button>
        </PageAction>
        <Box>
          <Layout>
            <Sider className={'sider-box'}>
              <Row style={{textAlign: "center"}} className={'row-sider'}>
                <ImgCrop grid rotate modalOk={"Cắt ảnh"} modalCancel={"Hủy"} modalTitle={"Chỉnh sửa hình ảnh"}
                         maxZoom={5} beforeCrop={this.checkFileImage} shape={'round'}>
                  <Upload showUploadList={false} beforeUpload={this.beforeUploadAvatar}
                          disabled={disableEdit}>
                    <Tooltip title={disableEdit ? '' : 'Nhấn để cập nhật ảnh đại diện'}>
                      <Avatar src={AnhHoSo !== "" ? AnhHoSo : GioiTinh ? defaultAvatar : defaultAvatar_Female}
                              size={120}/>
                    </Tooltip>
                  </Upload>
                </ImgCrop>
              </Row>
              <Row className={'row-name'}>
                {this.renderTenChucVu(ThongTinNhaKhoaHoc.ChucDanhKhoaHoc)} <b>{ThongTinNhaKhoaHoc.TenCanBo}</b>
              </Row>
              <Row>
                <Anchor affix={true} getContainer={() => document.getElementById("box-scroll")}
                        onClick={e => e.preventDefault()}>
                  <Anchor.Link href="#thongtincanhan" title="Thông tin cá nhân"/>
                  <Anchor.Link href="#quatrinhdaotao" title="Quá trình đào tạo"/>
                  <Anchor.Link href="#quatrinhcongtac" title="Quá trình công tác"/>
                  <Anchor.Link href="#ngoaingu" title="Ngoại ngữ"/>
                  <Anchor.Link href="#vanbangchungchi" title="Văn bằng chứng chỉ"/>
                  <Anchor.Link href="#giaithuongkhoahoc" title="Khen thưởng"/>
                  <Anchor.Link href="#huongnghiencuuchinh" title="Hướng nghiên cứu chính"/>
                  <Anchor.Link href="#duandetai" title="Dự án/ Đề tài"/>
                  <Anchor.Link href="#baibaotapchi" title="Bài báo trên tạp chí KHCN"/>
                  <Anchor.Link href="#sachchuyenkhao" title="Sách đã xuất bản"/>
                  <Anchor.Link href="#ketquacongbo" title="Kết quả đã công bố hoặc đăng ký khác"/>
                  <Anchor.Link href="#sanphamdaotao" title="Kết quả đào tạo"/>
                  <Anchor.Link href="#cacmongiangday" title="Các môn giảng dạy"/>
                  <Anchor.Link href="#hoatdongkhoahockhac" title="Các hoạt động khoa học khác"/>
                </Anchor>
              </Row>
            </Sider>
            <Content className={'content-box'}>
              <div className={'content-height'} id={'box-scroll'}>
                <Wrapper>
                  {/**/}
                  {/*Thông tin cá nhân*/}
                  {/**/}
                  <Row id={'thongtincanhan'} className={'row-title-first'}>Thông tin cá nhân</Row>
                  <div className={'action-btn'}>
                    {saveThongTinCaNhan ?
                      <Button type={'primary'} onClick={() => this.saveThongTinCaNhan()}
                              disabled={disableEdit || errAPI}
                              style={{marginLeft: 10, height: 30, width: 70}}>Lưu</Button> : ""}
                  </div>
                  <Row>
                    <table className={'table-form-chi-tiet'}>
                      <tr>
                        <td style={{width: '15%'}}>Mã cán bộ</td>
                        <td style={{width: '30%'}}>
                          <Input className={'no-border hover-input'} value={ThongTinNhaKhoaHoc.MaCB}
                                 onChange={value => this.changeInfoNhaKhoaHoc(value.target.value, 'MaCB')}
                                 disabled={disableEdit || CanBoTrongTruong}/>
                        </td>
                        <td style={{width: '10%'}} className={'no-border'}/>
                        <td style={{width: '15%'}}>Họ và tên</td>
                        <td style={{width: '30%'}}>
                          <Input className={'no-border hover-input'} value={ThongTinNhaKhoaHoc.TenCanBo}
                                 onChange={value => this.changeInfoNhaKhoaHoc(value.target.value, 'TenCanBo')}
                                 disabled={disableEdit || CanBoTrongTruong} style={{textTransform: 'capitalize'}}
                                 onKeyPress={this.inputTen}/>
                        </td>
                      </tr>
                      <tr>
                        <td>Ngày sinh</td>
                        <td>
                          <Datepicker style={{width: '100%'}} className={'no-border'} placeholder={''}
                                      format={'DD/MM/YYYY'}
                                      value={ThongTinNhaKhoaHoc.NgaySinh ? moment(ThongTinNhaKhoaHoc.NgaySinh, 'DD/MM/YYYY') : ""}
                                      onChange={value => this.changeInfoNhaKhoaHoc(value, 'NgaySinh')}
                                      disabled={disableEdit || CanBoTrongTruong} allowClear={false}
                          />
                        </td>
                        <td className={'no-border'}/>
                        <td>Giới tính</td>
                        <td>
                          <Group value={ThongTinNhaKhoaHoc.GioiTinh}
                                 onChange={value => this.changeInfoNhaKhoaHoc(value.target.value, 'GioiTinh')}
                                 disabled={disableEdit || CanBoTrongTruong}>
                            <Radio value={1}>Nam</Radio>
                            <Radio value={0}>Nữ</Radio>
                          </Group>
                        </td>
                      </tr>
                      <tr>
                        <td>Học hàm, học vị</td>
                        <td>
                          <Select style={{width: '100%'}} showSearch
                                  value={ThongTinNhaKhoaHoc.ChucDanhKhoaHoc ? ThongTinNhaKhoaHoc.ChucDanhKhoaHoc : []}
                                  mode={"multiple"}
                                  onChange={value => this.changeInfoNhaKhoaHoc(value, 'ChucDanhKhoaHoc')}
                                  disabled={disableEdit || CanBoTrongTruong}>
                            {DanhSachHocVi.map(item => (
                              <Option value={item.Id}>{item.Name}</Option>
                            ))}
                          </Select>
                        </td>
                        <td className={'no-border'}/>
                        <td>Chức danh hành chính</td>
                        <td>
                          <Select style={{width: '100%'}} showSearch
                                  value={ThongTinNhaKhoaHoc.ChucDanhHanhChinh ? ThongTinNhaKhoaHoc.ChucDanhHanhChinh : []}
                                  mode={"multiple"}
                                  onChange={value => this.changeInfoNhaKhoaHoc(value, 'ChucDanhHanhChinh')}
                                  disabled={disableEdit || CanBoTrongTruong}>
                            {DanhSachChucDanh.map(item => (
                              <Option value={item.Id}>{item.Name}</Option>
                            ))}
                          </Select>
                        </td>
                      </tr>
                      <tr>
                        <td>Cơ quan công tác</td>
                        <td>
                          <Input className={'no-border hover-input'} value={ThongTinNhaKhoaHoc.CoQuanCongTac}
                                 onChange={value => this.changeInfoNhaKhoaHoc(value.target.value, 'CoQuanCongTac')}
                                 disabled={disableEdit || CanBoTrongTruong}/>
                        </td>
                        <td className={'no-border'}/>
                        <td>Địa chỉ cơ quan</td>
                        <td>
                          <Input className={'no-border hover-input'} value={ThongTinNhaKhoaHoc.DiaChiCoQuan}
                                 onChange={value => this.changeInfoNhaKhoaHoc(value.target.value, 'DiaChiCoQuan')}
                                 disabled={disableEdit || CanBoTrongTruong}/>
                        </td>
                      </tr>
                      <tr>
                        <td>Khoa/ phòng ban</td>
                        <td>
                          <Select style={{width: '100%'}} showSearch
                                  value={ThongTinNhaKhoaHoc.PhongBanID > 0 ? ThongTinNhaKhoaHoc.PhongBanID : undefined}
                                  onChange={value => this.changeInfoNhaKhoaHoc(value, 'PhongBanID')} disabled>
                            {DanhSachPhongBan.map(item => (
                              <Option value={item.Id}>{item.Name}</Option>
                            ))}
                          </Select>
                        </td>
                        <td className={'no-border'}/>
                        <td>Email</td>
                        <td>
                          <Input className={'no-border hover-input'} value={ThongTinNhaKhoaHoc.Email}
                                 onChange={value => this.changeInfoNhaKhoaHoc(value.target.value, 'Email')}
                                 disabled={disableEdit || CanBoTrongTruong}/>
                        </td>
                      </tr>
                      <tr>
                        <td>Điện thoại</td>
                        <td>
                          <Input className={'no-border hover-input'} value={ThongTinNhaKhoaHoc.DienThoai}
                                 onChange={value => this.changeInfoNhaKhoaHoc(value.target.value, 'DienThoai')}
                                 disabled={disableEdit || CanBoTrongTruong} onKeyPress={this.inputNumber}/>
                        </td>
                        <td style={{width: '10%'}} className={'no-border'}/>
                        <td>Điện thoại di động</td>
                        <td>
                          <Input className={'no-border hover-input'} value={ThongTinNhaKhoaHoc.DienThoaiDiDong}
                                 onChange={value => this.changeInfoNhaKhoaHoc(value.target.value, 'DienThoaiDiDong')}
                                 disabled={disableEdit || CanBoTrongTruong} onKeyPress={this.inputNumber}/>
                        </td>
                      </tr>
                      <tr>
                        <td>Fax</td>
                        <td>
                          <Input className={'no-border hover-input'} value={ThongTinNhaKhoaHoc.Fax}
                                 onChange={value => this.changeInfoNhaKhoaHoc(value.target.value, 'Fax')}
                                 disabled={disableEdit || CanBoTrongTruong} onKeyPress={this.inputNumber}/>
                        </td>
                        <td style={{width: '10%'}} className={'no-border'}/>
                        <td>Trạng thái</td>
                        <td>
                          <Select style={{width: '100%'}} showSearch value={ThongTinNhaKhoaHoc.TrangThaiID}
                                  onChange={value => this.changeInfoNhaKhoaHoc(value, 'TrangThaiID')}
                                  disabled={disableEdit || CanBoTrongTruong}>
                            <Option value={0}>Đang làm việc</Option>
                            <Option value={1}>Đã chuyển công tác</Option>
                            <Option value={2}>Đã nghỉ hưu</Option>
                            <Option value={3}>Đang thử việc</Option>
                          </Select>
                        </td>
                      </tr>
                      <tr>
                        <td>Chuyên gia</td>
                        <td>
                          <Checkbox checked={ThongTinNhaKhoaHoc.LaChuyenGia}
                                    onChange={value => this.changeInfoNhaKhoaHoc(value.target.checked, 'LaChuyenGia')}
                                    disabled={disableEdit || CanBoTrongTruong}/>
                        </td>
                        <td style={{width: '10%'}} className={'no-border'}/>
                        <td>Url</td>
                        <td>
                          <Input
                            className={`no-border hover-input ${this.validUrl(ThongTinNhaKhoaHoc.Url) ? 'input-url' : ''}`}
                            value={ThongTinNhaKhoaHoc.Url}
                            onChange={value => this.changeInfoNhaKhoaHoc(value.target.value, 'Url')}
                            disabled={disableEdit || errAPI}
                            onDoubleClick={() => this.clickUrl(ThongTinNhaKhoaHoc.Url)}/>
                        </td>
                      </tr>
                      <tr>
                        <td>File giới thiệu</td>
                        <td>
                          <Upload multiple={true}
                                  beforeUpload={this.beforeUploadFileLyLich}
                                  showUploadList={false}>
                            <Button disabled={disableEdit || CanBoTrongTruong}>Chọn file</Button>
                          </Upload>
                          {this.renderFileGioiThieu(ThongTinNhaKhoaHoc.FileDinhKem)}
                        </td>
                        <td style={{width: '10%'}} className={'no-border'}/>
                        <td>Người giới thiệu</td>
                        <td>
                          <Select showSearch allowClear
                                  value={this.getValueNguoiGioiThieu(NguoiGioiThieu)}
                                  onChange={this.changeNguoiGioiThieu}
                                  disabled={disableEdit || errAPI} style={{width: '100%'}}
                                  mode={'multiple'}>
                            {DanhSachCanBo.map(item => (
                              <Option value={`${item.CanBoID}_${item.CoQuanID}`}>
                                {item.TenCanBo}
                              </Option>
                            ))}
                          </Select>
                        </td>
                      </tr>
                      <tr>
                        <td/>
                        <td/>
                        <td/>
                        <td/>
                        <td>
                          {this.getLinkGioiThieu()}
                        </td>
                      </tr>
                    </table>
                  </Row>
                </Wrapper>
                {/**/}
                {/*Quá trình đào tạo*/}
                {/**/}
                <Row id={'quatrinhdaotao'} className={'row-title'}>Quá trình đào tạo</Row>
                <div className={'action-btn'}>
                  <Button type={'primary'} style={{marginLeft: 10, height: 30, width: 70}}
                          onClick={() => this.openModalAdd(1)}
                          disabled={disableAdd || errAPI}>Thêm</Button>
                </div>
                {QuaTrinhDaoTao.length > 0 ? renderQuaTrinhDaoTao(QuaTrinhDaoTao, this.openModalEdit, this.deleteData, disableEdit, disableDelete) : this.renderEmpty()}
                <ModalAddEditQuaTrinhDaoTao visible={visibleModalQuaTrinhDaoTao} dataEdit={dataEditQuaTrinhDaoTao}
                                            actions={actions} onCancel={() => this.closeModal(1)} key={modalKey}
                                            onCreate={this.submitModalQuaTrinhDaoTao} loading={loading}
                                            CanBoID={ThongTinNhaKhoaHoc.CanBoID}
                                            CoQuanID={ThongTinNhaKhoaHoc.CoQuanID}/>
                {/**/}
                {/*Quá trình công tác*/}
                {/**/}
                <Row id={'quatrinhcongtac'} className={'row-title'}>Quá trình công tác</Row>
                <div className={'action-btn'}>
                  <Button type={'primary'} style={{marginLeft: 10, height: 30, width: 70}}
                          onClick={() => this.openModalAdd(2)}
                          disabled={disableAdd || errAPI}>Thêm</Button>
                </div>
                {QuaTrinhCongTac.length > 0 ? renderQuaTrinhCongTac(QuaTrinhCongTac, this.openModalEdit, this.deleteData, disableEdit, disableDelete) : this.renderEmpty()}
                <ModalAddEditQuaTrinhCongTac visible={visibleModalQuaTrinhCongTac} dataEdit={dataEditQuaTrinhCongTac}
                                             actions={actions} onCancel={() => this.closeModal(2)} key={modalKey}
                                             onCreate={this.submitModalQuaTrinhCongTac} loading={loading}
                                             CanBoID={ThongTinNhaKhoaHoc.CanBoID}
                                             CoQuanID={ThongTinNhaKhoaHoc.CoQuanID}/>
                {/**/}
                {/*Ngoại ngữ*/}
                {/**/}
                <Row id={'ngoaingu'} className={'row-title'}>Ngoại ngữ</Row>
                <div className={'action-btn'}>
                  <Button type={'primary'} style={{marginLeft: 10, height: 30, width: 70}}
                          onClick={() => this.openModalAdd(3)}
                          disabled={disableAdd || errAPI}>Thêm</Button>
                </div>
                {NgoaiNgu.length > 0 ? renderNgoaiNgu(NgoaiNgu, this.openModalEdit, this.deleteData, disableEdit, disableDelete) : this.renderEmpty()}
                <ModalAddEditNgoaiNgu visible={visibleModalNgoaiNgu} dataEdit={dataEditNgoaiNgu}
                                      actions={actions} onCancel={() => this.closeModal(3)} key={modalKey}
                                      onCreate={this.submitModalNgoaiNgu} loading={loading}
                                      CanBoID={ThongTinNhaKhoaHoc.CanBoID} CoQuanID={ThongTinNhaKhoaHoc.CoQuanID}/>
                {/**/}
                {/*Văn bằng chứng chỉ*/}
                {/**/}
                <Row id={'vanbangchungchi'} className={'row-title'}>Văn bằng chứng chỉ</Row>
                <div className={'action-btn'}>
                  <Button type={'primary'} style={{marginLeft: 10, height: 30, width: 70}}
                          onClick={() => this.openModalAdd(4)}
                          disabled={disableAdd || errAPI}>Thêm</Button>
                </div>
                {VanBangChungChi.length > 0 ? renderVanBangChungChi(VanBangChungChi, this.openModalEdit, this.deleteData, disableEdit, disableDelete) : this.renderEmpty()}
                <ModalAddEditVanBangChungChi visible={visibleModalVanBangChungChi} dataEdit={dataEditVanBangChungChi}
                                             actions={actions} onCancel={() => this.closeModal(4)} key={modalKey}
                                             onCreate={this.submitModalVanBangChungChi} loading={loading}
                                             CanBoID={ThongTinNhaKhoaHoc.CanBoID}
                                             CoQuanID={ThongTinNhaKhoaHoc.CoQuanID}/>
                {/**/}
                {/*Giải thưởng khoa học*/}
                {/**/}
                <Row id={'giaithuongkhoahoc'} className={'row-title'}>Khen thưởng</Row>
                <div className={'action-btn'}>
                  <Button type={'primary'} style={{marginLeft: 10, height: 30, width: 70}}
                          onClick={() => this.openModalAdd(5)}
                          disabled={disableAdd || errAPI}>Thêm</Button>
                </div>
                {GiaiThuongKhoaHoc.length > 0 ? renderGiaiThuongKhoaHoc(GiaiThuongKhoaHoc, this.openModalEdit, this.deleteData, disableEdit, disableDelete) : this.renderEmpty()}
                <ModalAddEditGiaiThuongKhoaHoc visible={visibleModalGiaiThuongKhoaHoc}
                                               dataEdit={dataEditGiaiThuongKhoaHoc}
                                               actions={actions} onCancel={() => this.closeModal(5)} key={modalKey}
                                               onCreate={this.submitModalGiaiThuongKhoaHoc} loading={loading}
                                               CanBoID={ThongTinNhaKhoaHoc.CanBoID}
                                               CoQuanID={ThongTinNhaKhoaHoc.CoQuanID}/>
                {/**/}
                {/*Hướng nghiên cứu chính*/}
                {/**/}
                <Row id={'huongnghiencuuchinh'} className={'row-title'}>Hướng nghiên cứu chính</Row>
                <div className={'action-btn'}>
                  {saveHuongNghienCuuChinh ?
                    <Button type={'primary'} onClick={() => this.saveHuongNghienCuuChinh()}
                            disabled={disableAdd || errAPI}
                            style={{marginLeft: 10, height: 30, width: 70}}>Lưu</Button> : ""}
                </div>
                <Input.TextArea value={HuongNghienCuuChinh.TenHuongNghienCuuChinh}
                                onChange={value => this.changeHuongNghienCuuChinh(value.target.value)}
                                autoSize={{minRows: 4}} className={'no-border hover-input'} disabled={disableEdit}/>
                {/**/}
                {/*Dự án đề tài*/}
                {/**/}
                <Row id={'duandetai'} className={'row-title'}>
                  Dự án/ Đề tài
                </Row>
                <div className={'action-btn'}>
                  <Button type={'primary'} style={{marginLeft: 10, height: 30, width: 70}}
                          onClick={() => this.openModalAdd(6)}
                          disabled={disableAdd || errAPI}>Thêm</Button>
                </div>
                {DuAnDeTai.length > 0 ? renderDuAnDeTai(DuAnDeTai, this.openModalEdit, this.deleteData, disableEdit, disableDelete) : this.renderEmpty()}
                <ModalAddEditDuAnDeTai visible={visibleModalDuAnDeTai} dataEdit={dataEditDuAnDeTai}
                                       actions={actions} onCancel={() => this.closeModal(6)} key={modalKey}
                                       onCreate={this.submitModalDuAnDeTai} loading={loading}
                                       CanBoID={ThongTinNhaKhoaHoc.CanBoID} CoQuanID={ThongTinNhaKhoaHoc.CoQuanID}/>
                {/**/}
                {/*Bài báo*/}
                {/**/}
                <Row id={'baibaotapchi'} className={'row-title'}>
                  Bài báo trên tạp chí KHCN
                  <Radio.Group style={{marginLeft: 70}} value={LoaiBaiBao}
                               onChange={value => this.setState({LoaiBaiBao: value.target.value})}>
                    <Radio value={1}>Bài báo quốc tế</Radio>
                    <Radio value={2}>Bài báo trong nước</Radio>
                    <Radio value={3}>Hội thảo quốc tế</Radio>
                    <Radio value={4}>Hội thảo trong nước</Radio>
                  </Radio.Group>
                </Row>
                <div className={'action-btn'}>
                  <Button type={'primary'} style={{marginLeft: 10, height: 30, width: 70}}
                          onClick={() => this.openModalAdd(7)}
                          disabled={disableAdd || errAPI}>Thêm</Button>
                </div>
                {BaiBaoTapChi.filter(item => item.LoaiBaiBao === LoaiBaiBao).length > 0 ? renderBaiBaoTapChi(BaiBaoTapChi.filter(item => item.LoaiBaiBao === LoaiBaiBao), this.openModalEdit, this.deleteData, disableEdit, disableDelete, DanhSachCanBo) : this.renderEmpty()}
                <ModalAddEditBaiBaoTapChi visible={visibleModalBaiBaoTapChi} dataEdit={dataEditBaiBaoTapChi}
                                          actions={actions} onCancel={() => this.closeModal(7)} key={modalKey}
                                          onCreate={this.submitModalBaiBaoTapChi} loading={loading}
                                          FileLimit={FileLimit} CoQuanID={ThongTinNhaKhoaHoc.CoQuanID}
                                          FileAllow={FileAllow} CanBoID={ThongTinNhaKhoaHoc.CanBoID}
                                          DanhSachDeTai={DanhSachDeTai} DanhSachPhongBan={DanhSachPhongBan}
                                          DanhSachLoaiNhiemVu={DanhSachLoaiNhiemVu} DanhSachCanBo={DanhSachCanBo}/>
                {/**/}
                {/*Sách chuyên khảo*/}
                {/**/}
                <Row id={'sachchuyenkhao'} className={'row-title'}>Sách đã xuất bản</Row>
                <div className={'action-btn'}>
                  <Button type={'primary'} style={{marginLeft: 10, height: 30, width: 70}}
                          onClick={() => this.openModalAdd(9)}
                          disabled={disableAdd || errAPI}>Thêm</Button>
                </div>
                {SachChuyenKhao.length > 0 ? renderSachChuyenKhao(SachChuyenKhao, this.openModalEdit, this.deleteData, disableEdit, disableDelete, DanhSachCanBo) : this.renderEmpty()}
                <ModalAddEditSachChuyenKhao visible={visibleModalSachChuyenKhao} dataEdit={dataEditSachChuyenKhao}
                                            actions={actions} onCancel={() => this.closeModal(9)} key={modalKey}
                                            onCreate={this.submitModalSachChuyenKhao} loading={loading}
                                            CanBoID={ThongTinNhaKhoaHoc.CanBoID} CoQuanID={ThongTinNhaKhoaHoc.CoQuanID}
                                            DanhSachDeTai={DanhSachDeTai} DanhSachCanBo={DanhSachCanBo}
                                            DanhSachLoaiNhiemVu={DanhSachLoaiNhiemVu}/>
                {/**/}
                {/*Kết quả đã công bố*/}
                {/**/}
                <Row id={'ketquacongbo'} className={'row-title'}>Kết quả nghiên cứu đã công bố hoặc đăng ký khác
                  <span style={{fontWeight: "normal"}}> (bằng phát minh, sáng chế/ giải pháp hữu ích,…)</span>
                </Row>
                <div className={'action-btn'}>
                  <Button type={'primary'} style={{marginLeft: 10, height: 30, width: 70}}
                          onClick={() => this.openModalAdd(8)}
                          disabled={disableAdd || errAPI}>Thêm</Button>
                </div>
                {KetQuaNghienCuu.length > 0 ? renderKetQuaNghienCuu2(KetQuaNghienCuu, this.openModalEdit, this.deleteData, disableEdit, disableDelete, DanhSachCanBo) : this.renderEmpty()}
                <ModalAddEditKetQuaNghienCuu visible={visibleModalKetQuaNghienCuu} dataEdit={dataEditKetQuaNghienCuu}
                                             actions={actions} onCancel={() => this.closeModal(8)} key={modalKey}
                                             onCreate={this.submitModalKetQuaNghienCuu} loading={loading}
                                             CanBoID={ThongTinNhaKhoaHoc.CanBoID} CoQuanID={ThongTinNhaKhoaHoc.CoQuanID}
                                             DanhSachDeTai={DanhSachDeTai} DanhSachLoaiNhiemVu={DanhSachLoaiNhiemVu}
                                             DanhSachCanBo={DanhSachCanBo}/>
                {/**/}
                {/*SẢn phẩm đào tạo*/}
                {/**/}
                <Row id={'sanphamdaotao'} className={'row-title'}>Kết quả đào tạo</Row>
                <div className={'action-btn'}>
                  <Button type={'primary'} style={{marginLeft: 10, height: 30, width: 70}}
                          onClick={() => this.openModalAdd(13)}
                          disabled={disableAdd || errAPI}>Thêm</Button>
                </div>
                {SanPhamDaoTao.length > 0 ? renderSanPhamDaoTao2(SanPhamDaoTao, this.openModalEdit, this.deleteData, disableEdit, disableDelete) : this.renderEmpty()}
                <ModalAddEditSanPhamDaoTao visible={visibleModalSanPhamDaoTao} dataEdit={dataEditSanPhamDaoTao}
                                           actions={actions} onCancel={() => this.closeModal(13)} key={modalKey}
                                           onCreate={this.submitModalSanPhamDaoTao} loading={loading}
                                           CanBoID={ThongTinNhaKhoaHoc.CanBoID} CoQuanID={ThongTinNhaKhoaHoc.CoQuanID}
                                           DanhSachLoaiNhiemVu={DanhSachLoaiNhiemVu} DanhSachDeTai={DanhSachDeTai}/>
                {/**/}
                {/*Các môn giảng dạy*/}
                {/**/}
                <Row id={'cacmongiangday'} className={'row-title'}>Các môn giảng dạy</Row>
                <div className={'action-btn'}>
                  <Button type={'primary'} style={{marginLeft: 10, height: 30, width: 70}}
                          onClick={() => this.openModalAdd(10)}
                          disabled={disableAdd || errAPI}>Thêm</Button>
                </div>
                {CacMonGiangDay.length > 0 ? renderCacMonGiangDay(CacMonGiangDay, this.openModalEdit, this.deleteData, disableEdit, disableDelete) : this.renderEmpty()}
                <ModalAddEditCacMonGiangDay visible={visibleModalCacMonGiangDay} dataEdit={dataEditCacMonGiangDay}
                                            actions={actions} onCancel={() => this.closeModal(10)} key={modalKey}
                                            onCreate={this.submitModalCacMonGiangDay} loading={loading}
                                            CanBoID={ThongTinNhaKhoaHoc.CanBoID} FileLimit={FileLimit}
                                            FileAllow={FileAllow}
                                            CoQuanID={ThongTinNhaKhoaHoc.CoQuanID}/>
                {/**/}
                {/*Hoạt động khoa học khác*/}
                {/**/}
                <Row id={'hoatdongkhoahockhac'} className={'row-title'}>Các hoạt động khoa học khác</Row>
                <div className={'action-btn'}>
                  <Button type={'primary'} style={{marginLeft: 10, height: 30, width: 70}}
                          onClick={() => this.openModalAdd(11)}
                          disabled={disableAdd || errAPI}>Thêm</Button>
                </div>
                {HoatDongKhoaHoc.length > 0 ? renderHoatDongKhoaHoc(HoatDongKhoaHoc, this.openModalEdit, this.deleteData, disableEdit, disableDelete) : this.renderEmpty()}
                <ModalAddEditHoatDongKhoaHoc visible={visibleModalHoatDongKhoaHoc} dataEdit={dataEditHoatDongKhoaHoc}
                                             actions={actions} onCancel={() => this.closeModal(11)} key={modalKey}
                                             onCreate={this.submitModalHoatDongKhoaHoc} loading={loading}
                                             CanBoID={ThongTinNhaKhoaHoc.CanBoID} FileLimit={FileLimit}
                                             FileAllow={FileAllow}
                                             DanhSachNhiemVu={DanhSachNhiemVuKhoaHoc}
                                             CoQuanID={ThongTinNhaKhoaHoc.CoQuanID}/>
              </div>
            </Content>
          </Layout>
        </Box>
        {/* DIV PRINT */}
        <div ref={this.formPrint} style={{display: 'none'}}>
          <div style={{fontFamiLy: 'Times New Roman'}}>
            {this.renderContentPrint(disableEdit, CanBoTrongTruong)}
          </div>
        </div>
        {/*END PRINT*/}
      </LayoutWrapper>
    )
  }
}

function mapStateToProps(state) {
  return {
    ...state.ChiTietNhaKhoaHoc
  };
}

export default connect(
  mapStateToProps,
  actions
)(ChiTietNhaKhoaHoc);