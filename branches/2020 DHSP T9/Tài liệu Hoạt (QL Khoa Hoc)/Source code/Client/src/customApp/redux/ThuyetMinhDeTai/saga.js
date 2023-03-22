import {all, takeEvery, put, call} from "redux-saga/effects";
import api from "../../containers/ThuyetMinhDeTai/config";
import apiConfig from "../../containers/ThamSoHeThong/config";
import apiCanBo from "../../containers/QLNhaKhoaHoc/config";
import actions from "./actions";
import {formatDataTreeSelect} from "../../../helpers/utility";

function* getInitData({payload}) {
  try {
    const capquanly = yield call(api.DanhSachCapQuanLy);
    const canbo = yield call(apiCanBo.DanhSachNhaKhoaHoc, {QuyenQuanLy: 0, PageSize: 9999999});
    const configfile = yield call(apiConfig.GetByKey, {ConfigKey: 'FILE_LIMIT'});
    const fileallow = yield call(apiConfig.GetByKey, {ConfigKey: 'LOAI_FILE'});
    const thuyetminh = yield call(api.DanhSachThuyetMinh, payload.filterData);
    yield put({
      type: actions.THUYETMINH_GET_INIT_DATA_REQUEST_SUCCESS,
      payload: {
        DanhSachCapQuanLy: formatDataTreeSelect(capquanly.data.Data),
        DanhSachCanBo: canbo.data.Data,
        FileLimit: configfile.data.Data.ConfigValue,
        FileAllow: fileallow.data.Data.ConfigValue,
        DanhSachThuyetMinh: thuyetminh.data.Data
      }
    });
  } catch (e) {
    yield put({
      type: actions.THUYETMINH_GET_INIT_DATA_REQUEST_ERROR
    });
  }
}

function* getList({payload}) {
  try {
    const thuyetminh = yield call(api.DanhSachThuyetMinh, payload.filterData);
    yield put({
      type: actions.THUYETMINH_GET_LIST_REQUEST_SUCCESS,
      payload: {
        DanhSachThuyetMinh: thuyetminh.data.Data
      }
    });
  } catch (e) {
    yield put({
      type: actions.THUYETMINH_GET_LIST_REQUEST_ERROR
    });
  }
}

export default function* rootSaga() {
  yield all([yield takeEvery(actions.THUYETMINH_GET_INIT_DATA_REQUEST, getInitData)]);
  yield all([yield takeEvery(actions.THUYETMINH_GET_LIST_REQUEST, getList)]);
}
