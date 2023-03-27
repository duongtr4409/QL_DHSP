
const actions = {
  BAOCAO_GET_DATA_REQUEST: 'BAOCAO_GET_DATA_REQUEST',
  BAOCAO_GET_DATA_REQUEST_SUCCESS: 'BAOCAO_GET_DATA_REQUEST_SUCCESS',
  BAOCAO_GET_DATA_REQUEST_ERROR: 'BAOCAO_GET_DATA_REQUEST_ERROR',
  getInitData: () => {
    return (disPatch) => {
      disPatch({
        type: actions.BAOCAO_GET_DATA_REQUEST
      });
    }
  }
};
export default actions;