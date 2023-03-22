import React from "react";
import {Button, Checkbox, Collapse, Icon, message, Modal, Spin, Table} from "antd";
import {connect} from "react-redux";
import {convertFileTable, getRoleByKey} from "../../../helpers/utility";
import * as actions from "../../redux/QLDeXuat/actions";
import GoInput from "../../components/GoInput/index";
import GoSelect from "../../components/GoSelect/index";
import GoEditor from "../../components/GoEditor/editor";
import {GoDatePicker, GoTreeSelect} from "../../components/index";
import {ValidatorForm} from "react-form-validator-core";
import api from "../QLDeXuat/config";
import * as moment from "moment";
import LayoutWrapper from "../../../components/utility/layoutWrapper";
import PageHeader from "../../../components/utility/pageHeader";
import Box from "../../../components/utility/box";
import apiThuyetMinh from "./config";
import apiCanBo from "../QLNhaKhoaHoc/config";
import {withAPI} from "../../components/withAPI";
import BoxTable from "./BoxTable.style";

const TreeSelectWithApi = withAPI(GoTreeSelect);
const {Panel} = Collapse;
const SelectWithApi = withAPI(GoSelect);

class ChiTietDeXuatThuyetMinh extends React.Component {
  modal = null;
  Id;

  constructor(props) {
    document.title = "Chi tiết đề xuất đề tài";
    super(props);
    this.submitBtn = React.createRef();
    this.isBackButtonClicked = false;
    this.state = {
      confirmLoading: false,
      dataDeXuat: {
        MaDeXuat: "",
        CoQuanID: "",
        TenDeXuat: "",
        LinhVucKinhTeXaHoi: null,
        LinhVucNghienCuu: null,
        CapQuanLy: null,
        NguoiDeXuat: null,
        KinhPhiDuKien: null,
        ThoiGianNghienCuu: null,
        TinhCapThiet: "",
        MucTieu: "",
        SanPham: "",
        FileDinhKem: [],
        NgayDeXuat: null,
        NoiDung: null,
        DiaChiUngDung: "",
        ThoiGianThucHienTu: "",
        ThoiGianThucHienDen: "",
        ThuocChuongTrinh: "",
      },
      loading: true,
      DSLoaiKQ: [
        {
          text: "Bài báo",
          value: "Baibao",
        },
      ],
      DanhSachThuyetMinh: [],
      DanhSachCanBo: []
    };
  }

  componentDidMount() {
    this.onInitData();
  }

  onInitData = () => {
    const {id} = this.props.match.params;
    this.Id = id;
    api.chiTietDeXuat({DeXuatID: id}).then((res) => {
      if (res.data && res.data.Status === 1) {
        this.setState({dataDeXuat: res.data.Data});
        apiThuyetMinh.GetAllDanhSachThuyetMinh({DeXuatID: id}).then(restm => {
          if (restm.data && restm.data.Status === 1) {
            this.setState({DanhSachThuyetMinh: restm.data.Data});
            apiCanBo.DanhSachNhaKhoaHoc({QuyenQuanLy: 0, PageSize: 9999999}).then(rescb => {
              if (restm.data && restm.data.Status === 1) {
                this.setState({DanhSachCanBo: rescb.data.Data, loading: false});
              } else {
                this.setState({loading: false});
                message.error(`${rescb.data && rescb.data.Message ? rescb.data.Message : "Lấy chi tiết đề xuất thất bại"}`);
              }
            })
          } else {
            this.setState({loading: false});
            message.error(`${restm.data && restm.data.Message ? restm.data.Message : "Lấy chi tiết đề xuất thất bại"}`);
          }
        });
      } else {
        this.setState({loading: false});
        message.error(`${res.data && res.data.Message ? res.data.Message : "Lấy chi tiết đề xuất thất bại"}`);
      }
    });
  };

  handleSubmit = () => {
  };

  renderFileTable = (data) => {
    const columns = [
      {
        title: "STT",
        dataIndex: "index",
        key: "index",
        render: (text, record, index) => <p>{index + 1}</p>,
      },
      {
        title: "Cán bộ thực hiện",
        dataIndex: "TenNguoiTao",
        key: "TenNguoiTao",
      },
      {
        title: "Ngày thực hiện",
        dataIndex: "NgayTao",
        key: "NgayTao",
        render: (text, record, index) => <p>{moment(text).format("DD/MM/YYYY")}</p>,
      },
      {
        title: "Ghi chú",
        dataIndex: "NoiDung",
        key: "NoiDung",
      },
      {
        title: "File đính kèm",
        dataIndex: "files",
        key: "files",
        render: (text, record, index) => (
          <div>
            {record.files.map((item) => (
              <span className="border border-primary rounded mx-1 my-1 d-inline-block p-1">
                <a download={item.TenFileGoc} target="_blank" href={item.FileUrl}>
                  {item.TenFileGoc}
                </a>
              </span>
            ))}
          </div>
        ),
      },
    ];
    return <Table locale={{emptyText: "Không có dữ liệu"}} pagination={false} rowKey="FileDinhKemID" bordered
                  dataSource={data} columns={columns}/>;
  };

  renderTrangThai = (status) => {
    return status < 2 ? status === 0 ? 'Chờ duyệt' : 'Đã duyệt' : 'Không duyệt';
  };

  renderTenCanBo = (CanBoID, CoQuanID) => {
    const {DanhSachCanBo} = this.state;
    const canbo = DanhSachCanBo.filter(item => item.CanBoID === CanBoID && item.CoQuanID === CoQuanID)[0];
    if (canbo) {
      return canbo.TenCanBo;
    }
  };

  renderTable = () => {
    const {DanhSachThuyetMinh} = this.state;
    return DanhSachThuyetMinh.map((item, index) =>
      <tr>
        <td style={{textAlign: "center", width: '5%'}}>{index + 1}</td>
        <td style={{width: '40%'}}>
          <a href={item.FileThuyetMinh.FileUrl} target={'_blank'}>{item.FileThuyetMinh.TenFileGoc}</a>
        </td>
        <td style={{width: '35%'}}>{this.renderTenCanBo(item.CanBoID, item.CoQuanID)}</td>
        <td style={{textAlign: "center", width: '20%'}}>
          {this.renderTrangThai(item.TrangThai)}
        </td>
      </tr>
    )
  };

  render() {
    const {dataDeXuat} = this.state;
    const {FileDinhKem, FileThuyetMinh} = dataDeXuat;
    const groupedFileDinhKem = convertFileTable(FileThuyetMinh, ["FileDinhKemID", "TenFileGoc", "FileUrl", "NgayTao", "TenNguoiTao", "NoiDung"]);
    return (
      <LayoutWrapper>
        <PageHeader>Chi tiết đề xuất thuyết minh</PageHeader>
        <Box>
          <Spin spinning={this.state.loading}>
            <div className="custom-collapse">
              <Collapse defaultActiveKey={["1", "2"]} expandIconPosition="right">
                <Panel header="Thông tin đề xuất đề tài" key="1">
                  <ValidatorForm
                    ref="form"
                    onSubmit={this.handleSubmit}
                    onError={() => {
                      const firstError = document.getElementsByClassName("invalid-error")[0];
                      if (firstError) {
                        firstError.scrollIntoView({behavior: "smooth", block: "end", inline: "nearest"});
                      }
                    }}
                  >
                    <div className="row ">
                      <div className="col-lg-6 col-12 my-1  ">
                        <div className="row align-items-center">
                          <div className="col-6 col-lg-4 ">Mã đề xuất</div>
                          <div className="col-6 col-lg-8 ">
                            <GoInput
                              disabled={true}
                              withTextToInput
                              value={dataDeXuat.MaDeXuat}
                            />
                          </div>
                        </div>
                      </div>
                      <div className="col-lg-6 col-12 my-1  ">
                        <div className="row align-items-center">
                          <div className="col-6 col-lg-4 ">
                            Tên đề xuất <span className="text-danger">*</span>
                          </div>
                          <div className="col-6 col-lg-8 ">
                            <GoInput
                              disabled={true}
                              withTextToInput
                              value={dataDeXuat.TenDeXuat}
                              validators={["required"]}
                              errorMessages={["Nội dung bắt buộc"]}
                            />
                          </div>
                        </div>
                      </div>
                      <div className="col-lg-6 col-12 my-1  ">
                        <div className="row align-items-center">
                          <div className="col-6 col-lg-4 ">Lĩnh vực nghiên cứu KHCN</div>
                          <div className="col-6 col-lg-8 ">
                            <TreeSelectWithApi
                              disabled={true}
                              withTextToInput
                              apiConfig={{
                                api: api.danhSachCayLinhVuc,
                                valueField: "ID",
                                nameField: "Name",
                                codeField: "Code",
                                filter: {
                                  Type: 1,
                                  status: true,
                                },
                              }}
                              value={dataDeXuat.LinhVucNghienCuu === 0 ? null : dataDeXuat.LinhVucNghienCuu}
                            />
                          </div>
                        </div>
                      </div>
                      <div className="col-lg-6 col-12 my-1  ">
                        <div className="row align-items-center">
                          <div className="col-6 col-lg-4 ">Lĩnh vực kinh tế - xã hội</div>
                          <div className="col-6 col-lg-8 ">
                            <TreeSelectWithApi
                              disabled={true}
                              withTextToInput
                              apiConfig={{
                                api: api.danhSachCayLinhVuc,
                                valueField: "ID",
                                nameField: "Name",
                                codeField: "Code",
                                filter: {
                                  Type: 2,
                                  status: true,
                                },
                              }}
                              allowClear
                              showSearch
                              value={dataDeXuat.LinhVucKinhTeXaHoi === 0 ? null : dataDeXuat.LinhVucKinhTeXaHoi}
                            />
                          </div>
                        </div>
                      </div>

                      <div className="col-lg-6 col-12 my-1  ">
                        <div className="row align-items-center">
                          <div className="col-6 col-lg-4 ">Cấp quản lý</div>
                          <div className="col-6 col-lg-8 ">
                            <TreeSelectWithApi
                              disabled={true}
                              withTextToInput
                              apiConfig={{
                                api: api.danhSachCayCapQuanly,
                                valueField: "ID",
                                nameField: "Name",
                              }}
                              value={dataDeXuat.CapQuanLy === 0 ? null : dataDeXuat.CapQuanLy}
                            />
                          </div>
                        </div>
                      </div>
                      <div className="col-lg-6 col-12 my-1  ">
                        <div className="row align-items-center">
                          <div className="col-6 col-lg-4 ">
                            Người đề xuất <span className="text-danger">*</span>
                          </div>
                          <div className="col-6 col-lg-8">
                            <SelectWithApi
                              disabled={true}
                              withTextToInput
                              apiConfig={{
                                api: api.DanhSachTaiKhoan,
                                valueField: "CanBoID",
                                nameField: "TenCanBo",
                                filter: {
                                  PageSize: 20000,
                                },
                              }}
                              useSearchAPI
                              value={dataDeXuat.NguoiDeXuat}
                              returnFullItem
                              validators={["required"]}
                              errorMessages={["Nội dung bắt buộc"]}
                            />
                          </div>
                        </div>
                      </div>

                      <div className="col-12 my-1  ">
                        <div className="row align-items-center">
                          <div className="col-2 ">Tính cấp thiết</div>
                          <div className="col-10 ">
                            <GoEditor disabled={true} value={dataDeXuat.TinhCapThiet}/>
                          </div>
                        </div>
                      </div>
                      <div className="col-12 my-1">
                        <div className=" row align-items-center">
                          <div className=" col-lg-2">Mục tiêu</div>
                          <div className="col-lg-10 ">
                            <GoEditor disabled={true} value={dataDeXuat.MucTieu}/>
                          </div>
                        </div>
                      </div>
                      <div className="col-12 my-1">
                        <div className=" row align-items-center">
                          <div className=" col-lg-2">Sản phẩm</div>
                          <div className="col-lg-10 ">
                            <GoEditor disabled={true} value={dataDeXuat.SanPham}/>
                          </div>
                        </div>
                      </div>

                      <div className="col-lg-6 col-12 my-1  ">
                        <div className="row align-items-center">
                          <div className="col-6 col-lg-4 ">Kinh phí dự kiến</div>
                          <div className="col-6 col-lg-8 ">
                            <GoInput
                              disabled={true}
                              withTextToInput
                              isNumber
                              currency="VNĐ"
                              value={dataDeXuat.KinhPhiDuKien}
                              validators={["isNumber", "isPositive"]}
                              errorMessages={["Kinh phí dự kiến phải là dạng số", "Kinh phí dự kiến phải lớn hơn 0"]}
                            />
                            <div>VNĐ</div>
                          </div>
                        </div>
                      </div>
                      <div className="col-lg-6 col-12 my-1  ">
                        <div className="row align-items-center">
                          <div className="col-6 col-lg-4 ">Thời gian nghiên cứu</div>
                          <div className="col-6 col-lg-8 ">
                            <GoSelect
                              withTextToInput
                              disabled={true}
                              value={dataDeXuat.ThoiGianNghienCuu}
                              data={[
                                {
                                  value: 12,
                                  text: "12 tháng",
                                },
                                {
                                  value: 18,
                                  text: "18 tháng",
                                },
                                {
                                  value: 24,
                                  text: "24 tháng",
                                },
                                {
                                  value: 36,
                                  text: "36 tháng",
                                },
                              ]}
                            />
                          </div>
                        </div>
                      </div>
                      <div className="col-lg-6 col-12 my-1  ">
                        <div className="row align-items-center">
                          <div className="col-6 col-lg-4 ">Ngày đề xuất</div>
                          <div className="col-6 col-lg-8 ">
                            <GoDatePicker
                              disabled={true}
                              withTextToInput
                              value={dataDeXuat.NgayDeXuat ? moment(dataDeXuat.NgayDeXuat, "YYYY-MM-DD") : dataDeXuat.NgayDeXuat}
                              format="DD/MM/YYYY"
                            />
                          </div>
                        </div>
                      </div>
                      <div className="col-lg-6 col-12 my-1  ">
                        <div className="row align-items-center">
                          <div className="col-6 col-lg-4 ">Thời gian thực hiện</div>
                          <div className="col-6 col-lg-8 ">
                            <div className="row justify-content-between align-items-center">
                              <div className="col-5">
                                <GoDatePicker
                                  monthPicker
                                  placeholder=""
                                  disabled={true}
                                  withTextToInput
                                  value={dataDeXuat.ThoiGianThucHienTu ? moment(dataDeXuat.ThoiGianThucHienTu, "MM/YYYY") : dataDeXuat.ThoiGianThucHienTu}
                                  format="MM/YYYY"
                                />
                              </div>
                              <div className="col-2 px-0 text-center"> đến</div>
                              <div className="col-5">
                                <GoDatePicker
                                  monthPicker
                                  placeholder=""
                                  disabled={true}
                                  withTextToInput
                                  value={dataDeXuat.ThoiGianThucHienDen ? moment(dataDeXuat.ThoiGianThucHienDen, "MM/YYYY") : dataDeXuat.ThoiGianThucHienDen}
                                  format="MM/YYYY"
                                />
                              </div>
                            </div>
                          </div>
                        </div>
                      </div>
                      <div className=" col-12 my-1  ">
                        <div className="row align-items-center">
                          <div className="col-6 col-lg-2 ">Nội dung</div>
                          <div className="col-6 col-lg-10  ">
                            <GoInput rows={3} isTextArea disabled={true} withTextToInput value={dataDeXuat.NoiDung}/>
                          </div>
                        </div>
                      </div>
                      <div className="col-lg-6 col-12 my-1  ">
                        <div className="row align-items-center">
                          <div className="col-6 col-lg-4 ">Thuộc chương trình</div>
                          <div className="col-6 col-lg-8 ">
                            <GoInput disabled={true} withTextToInput value={dataDeXuat.ThuocChuongTrinh}/>
                          </div>
                        </div>
                      </div>
                      <div className="col-lg-6 col-12 my-1  ">
                        <div className="row align-items-center">
                          <div className="col-6 col-lg-4 ">Địa chỉ ứng dụng</div>
                          <div className="col-6 col-lg-8 ">
                            <GoInput disabled={true} withTextToInput value={dataDeXuat.DiaChiUngDung}/>
                          </div>
                        </div>
                      </div>
                      <div className="col-12 my-1  ">
                        <div className="row align-items-center">
                          <div className="col-12 mb-1">
                            File đính kèm
                            <div className="clearfix"/>
                          </div>
                        </div>
                        <div>{this.renderFileTable(groupedFileDinhKem)}</div>
                      </div>
                    </div>
                  </ValidatorForm>
                </Panel>
                <Panel header={"Thông tin thuyết minh đề tài"} key={"2"}>
                  <BoxTable>
                    <table className='table-head'>
                      <tr>
                        <th style={{width: '5%'}}>STT</th>
                        <th style={{width: '40%'}}>Thuyết minh đề tài</th>
                        <th style={{width: '35%'}}>Chủ nhiệm đề tài</th>
                        <th style={{width: '20%'}}>Trạng thái</th>
                      </tr>
                    </table>
                    <div className='scroll' style={{maxHeight: 400}}>
                      <table className='table-scroll'>
                        {this.renderTable()}
                      </table>
                    </div>
                  </BoxTable>
                </Panel>
              </Collapse>
            </div>
          </Spin>
          <div className="text-center my-1">
            <Button
              onClick={() => {
                this.props.history.goBack();
              }}
            >
              Quay lại
            </Button>
          </div>
        </Box>
      </LayoutWrapper>
    );
  }
}

function mapStateToProps(state) {
  state.ThuyetMinhDeTai.role = getRoleByKey("thuyet-minh-de-tai");
  return {
    ...state.ThuyetMinhDeTai,
  };
}

export default connect(mapStateToProps, actions)(ChiTietDeXuatThuyetMinh);
