import { all, takeEvery, put, call } from "redux-saga/effects";
import api from "../../containers/DonViNghienCuu/config";
import actions from "./actions";
import apiLinhVuc from "../../containers/DMLinhVuc/config";
import {formatDataTreeSelect} from "../../../helpers/utility";

function* getInitData({payload}) {
  try {
    const response = yield call(api.DanhSachDonVi, payload.filterData);
    const linhvuc = yield call(apiLinhVuc.DanhSachLinhVucGroup, {Keyword: ""});
    yield put({
      type: actions.DONVINGHIENCUU_GET_INIT_DATA_REQUEST_SUCCESS,
      payload: {
        DanhSachDonViNghienCuu: response.data.Data,
        DanhSachLinhVuc: formatDataTreeSelect(linhvuc.data.Data),
        TotalRow: response.data.TotalRow
      }
    });
  } catch (e) {
    yield put({
      type: actions.DONVINGHIENCUU_GET_INIT_DATA_REQUEST_ERROR
    });
  }
}

function* getList({payload}) {
  try {
    const response = yield call(api.DanhSachDonVi, payload.filterData);
    yield put({
      type: actions.DONVINGHIENCUU_GET_LIST_REQUEST_SUCCESS,
      payload: {
        DanhSachDonViNghienCuu: response.data.Data,
        TotalRow: response.data.TotalRow
      }
    });
  } catch (e) {
    yield put({
      type: actions.DONVINGHIENCUU_GET_LIST_REQUEST_ERROR
    });
  }
}

export default function* rootSaga() {
  yield all([yield takeEvery(actions.DONVINGHIENCUU_GET_INIT_DATA_REQUEST, getInitData)]);
  yield all([yield takeEvery(actions.DONVINGHIENCUU_GET_LIST_REQUEST, getList)]);
}
