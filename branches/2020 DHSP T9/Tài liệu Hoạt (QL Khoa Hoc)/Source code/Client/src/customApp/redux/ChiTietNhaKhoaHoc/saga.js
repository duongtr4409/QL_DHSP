import {all, takeEvery, put, call} from "redux-saga/effects";
import api from "../../containers/ChiTietNhaKhoaHoc/config";
import apiCore from "../../containers/DataCoreAPI/config";
import apiConfig from "../../containers/ThamSoHeThong/config";
import apiCanBo from "../../containers/QLNhaKhoaHoc/config";
import apiKetQua from "../../containers/DMLoaiKetQua/config";
import actions from "./actions";
import {formatDataTreeSelect} from "../../../helpers/utility";

function* getInitData({payload}) {
  try {
    const chucdanh = yield call(apiCore.DanhSachChucVu);
    const phongban = yield call(apiCore.DanhSachPhongBan, {type: 0});
    const hocvi = yield call(apiCore.DanhSachHocVi);
    const nhiemvukhoahoc = yield call(apiCore.DanhSachNhiemVuKhoaHoc);
    const configfile = yield call(apiConfig.GetByKey, {ConfigKey: 'FILE_LIMIT'});
    const detai = yield call(api.DeTaiByCanBoID, {CanBoID: payload.filterData.CanBoID});
    const fileallow = yield call(apiConfig.GetByKey, {ConfigKey: 'LOAI_FILE'});
    const loainhiemvu = yield call(apiCore.DanhSachNhiemVu2);
    const canbo = yield call(apiCanBo.DanhSachNhaKhoaHoc, {QuyenQuanLy: 0, PageSize: 9999999});
    const configcanbongoaitruong = yield call(apiConfig.GetByKey, {ConfigKey : 'ID_COQUAN_NGOAITRUONG'});
    const ketqua = yield call(apiKetQua.DanhSachLoaiKetQuaGroup);
    yield put({
      type: actions.CHITIETNHAKHOAHOC_GET_INIT_DATA_REQUEST_SUCCESS,
      payload: {
        DanhSachChucDanh: chucdanh.data.Data,
        DanhSachPhongBan: phongban.data.Data,
        DanhSachHocVi: hocvi.data.Data,
        DanhSachNhiemVuKhoaHoc: formatDataTreeSelect(nhiemvukhoahoc.data.Data, false),
        DanhSachDeTai: detai.data.Data,
        FileLimit: configfile.data.Data.ConfigValue,
        FileAllow: fileallow.data.Data.ConfigValue,
        DanhSachLoaiNhiemVu: formatDataTreeSelect(loainhiemvu.data.Data, false),
        DanhSachCanBo: canbo.data.Data,
        CanBoNgoaiTruong: configcanbongoaitruong.data.Data.ConfigValue,
        DanhSachLoaiKetQua: ketqua.data.Data
      }
    });
  } catch (e) {
    yield put({
      type: actions.CHITIETNHAKHOAHOC_GET_INIT_DATA_REQUEST_ERROR
    });
  }
}

function* getList({payload}) {
  try {

    yield put({
      type: actions.CHITIETNHAKHOAHOC_GET_LIST_REQUEST_SUCCESS,
      payload: {}
    });
  } catch (e) {
    yield put({
      type: actions.CHITIETNHAKHOAHOC_GET_LIST_REQUEST_ERROR
    });
  }
}

export default function* rootSaga() {
  yield all([yield takeEvery(actions.CHITIETNHAKHOAHOC_GET_INIT_DATA_REQUEST, getInitData)]);
  yield all([yield takeEvery(actions.CHITIETNHAKHOAHOC_GET_LIST_REQUEST, getList)]);
}
