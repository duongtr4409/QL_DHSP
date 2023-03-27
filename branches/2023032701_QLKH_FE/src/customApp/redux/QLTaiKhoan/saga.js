import { all, takeEvery, put, call } from "redux-saga/effects";
import api from "../../containers/QLTaiKhoan/config";
import apiNhomNguoiDung from "../../containers/QLPhanQuyen/config";
import apiConfig from "../../containers/ThamSoHeThong/config";
import actions from "./actions";

const paramAll = { PageSize: 999999, PageNumber: 1 };

function* getInitData({ payload }) {
  try {
    const response = yield call(api.DanhSachTaiKhoan, payload.filterData);
    const responseAllNguoiDung = yield call(api.DanhSachTaiKhoan, paramAll);
    const responseCoQuan = yield call(api.danhSachCoQuan);
    const responsePhanQuyen = yield call(apiNhomNguoiDung.danhSachNhom, paramAll);
    const responseConfig = yield call(apiConfig.GetByKey, { ConfigKey: "MatKhau_MacDinh" });
    const filelimit = yield call(apiConfig.GetByKey, { ConfigKey: "FILE_LIMIT" });

    yield put({
      type: actions.TAIKHOAN_GET_INIT_DATA_REQUEST_SUCCESS,
      payload: {
        DanhSachTaiKhoan: response.data.Data,
        DanhSachTaiKhoanAll: responseAllNguoiDung.data.Data,
        TotalRow: response.data.TotalRow,
        DanhSachNhomNguoiDung: responsePhanQuyen.data.Data,
        MatKhauMacDinh: responseConfig.data.Data.ConfigValue,
        FileLimit: filelimit.data.Data.ConfigValue,
        DanhSachCoQuan: responseCoQuan.data.Data
      },
    });
  } catch (e) {
    console.log(e);
    yield put({
      type: actions.TAIKHOAN_GET_INIT_DATA_REQUEST_ERROR,
    });
  }
}

function* getList({ payload }) {
  try {
    const response = yield call(api.DanhSachTaiKhoan, payload.filterData);
    yield put({
      type: actions.TAIKHOAN_GET_LIST_REQUEST_SUCCESS,
      payload: {
        DanhSachTaiKhoan: response.data.Data,
        TotalRow: response.data.TotalRow,
      },
    });
  } catch (e) {
    yield put({
      type: actions.TAIKHOAN_GET_LIST_REQUEST_ERROR,
    });
  }
}

export default function* rootSaga() {
  yield all([yield takeEvery(actions.TAIKHOAN_GET_INIT_DATA_REQUEST, getInitData)]);
  yield all([yield takeEvery(actions.TAIKHOAN_GET_LIST_REQUEST, getList)]);
}
