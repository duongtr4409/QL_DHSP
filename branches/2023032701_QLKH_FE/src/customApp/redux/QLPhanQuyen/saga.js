import { all, takeEvery, put, call } from "redux-saga/effects";
import api from "../../containers/QLPhanQuyen/config";
import actions from "./actions";

function* getInitData({ payload }) {
  try {
    const response = yield call(api.danhSachNhom, payload.filterData);
    //danh sach co quan
    // const responseCoQuan = yield call(api.danhSachCoQuan, payload.filterData);
    // const DanhSachCoQuan = yield formatData(responseCoQuan.data.Data);
    //danh sach chuc nang
    //danh sach  can bo
    // const responseCanBo = yield call(api.danhSachCanBo, { CoquanID: payload.filterData.CoQuanID });
    // const DanhSachCanBo = responseCanBo.data.Data ? responseCanBo.data.Data : [];
    yield put({
      type: actions.PHANQUYEN_GET_INIT_DATA_REQUEST_SUCCESS,
      payload: {
        DanhSachNhom: response.data.Data,
        // DanhSachCoQuan,
        // DanhSachCanBo,
        //DanhSachChucNang,
        TotalRow: response.data.TotalRow,
      },
    });
  } catch (e) {
    yield put({
      type: actions.PHANQUYEN_GET_INIT_DATA_REQUEST_ERROR,
    });
  }
}

function* getList({ payload }) {
  try {
    const response = yield call(api.danhSachNhom, payload.filterData);
    //danh sach  can bo
    // const responseCanBo = yield call(api.danhSachCanBo, { CoquanID: payload.filterData.CoQuanID });
    // const DanhSachCanBo = responseCanBo.data.Data ? responseCanBo.data.Data : [];
    yield put({
      type: actions.PHANQUYEN_GET_LIST_REQUEST_SUCCESS,
      payload: {
        DanhSachNhom: response.data.Data,
        // DanhSachCanBo,
        TotalRow: response.data.TotalRow,
      },
    });
  } catch (e) {
    yield put({
      type: actions.PHANQUYEN_GET_LIST_REQUEST_ERROR,
    });
  }
}

function formatData(DanhSachCoQuan) {
  const DanhSach = DanhSachCoQuan.map((value1, index1) => {
    //-------1
    let title1 = value1.Ten;
    let key1 = `${index1}`;
    let valueSelect1 = `${value1.ID}`;
    let children1 = null;
    if (value1.Children) {
      children1 = value1.Children.map((value2, index2) => {
        //------2
        let title2 = value2.Ten;
        let key2 = `${index1}-${index2}`;
        let valueSelect2 = `${value2.ID}`;
        let children2 = null;
        if (value2.Children) {
          children2 = value2.Children.map((value3, index3) => {
            //------3
            let title3 = value3.Ten;
            let key3 = `${index1}-${index2}-${index3}`;
            let valueSelect3 = `${value3.ID}`;
            let children3 = null;
            return {
              ...value3,
              title: title3,
              key: key3,
              value: valueSelect3,
              children: children3,
            };
          });
        }
        return {
          ...value2,
          title: title2,
          key: key2,
          value: valueSelect2,
          children: children2,
        };
      });
    }
    return {
      ...value1,
      title: title1,
      key: key1,
      value: valueSelect1,
      children: children1,
    };
  });
  return DanhSach;
}

export default function* rootSaga() {
  yield all([yield takeEvery(actions.PHANQUYEN_GET_INIT_DATA_REQUEST, getInitData)]);
  yield all([yield takeEvery(actions.PHANQUYEN_GET_LIST_REQUEST, getList)]);
}
