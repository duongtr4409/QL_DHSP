import React from "react";
import { Modal, Icon, Button, Input, Divider, Upload } from "antd";
import Constants, { MODAL_NORMAL, ITEM_LAYOUT3, REQUIRED } from "../../../settings/constants";
import GoInput from "../../components/GoInput/index";
import GoSelect from "../../components/GoSelect/index";
import GoEditor from "../../components/GoEditor/editor";
import { ValidatorForm } from "react-form-validator-core";

class ThemCanBoModal extends React.Component {
  constructor(props) {
    super(props);
    this.submitBtn = React.createRef();
    this.state = {
      confirmLoading: false,
      dataCanBo: {
        avatar: "",
        MaDeXuat: "",
        LoaiKQ: "",
        NoiDungKQNC: "",
        Kinhphi: 0,
        members: [],
        Files: [],
      },
    };
  }
  handleCancel = () => {
    this.props.onClose();
  };
  handleOk = () => {
    // this.setState({ confirmLoading: true });
    this.submitBtn.current.click();
  };
  handleSubmit = () => {};
  handleInputChange = (name) => (event) => {
    const { dataDeXuat } = this.state;
    dataDeXuat[name] = event.target ? event.target.value : event;
    this.setState({
      dataDeXuat,
    });
  };
  handleSelectChange = (name) => (value) => {
    const { dataDeXuat } = this.state;
    dataDeXuat[name] = value;
    this.setState({
      dataDeXuat,
    });
  };
  handleUploadAvatar = (info) => {};
  render() {
    const { confirmLoading, dataCanBo } = this.state;
    return (
      <Modal zIndex={1001} confirmLoading={confirmLoading} width={1000} title={"Thêm cán bộ"} visible={true} onOk={this.handleOk} onCancel={this.handleCancel}>
        <ValidatorForm ref="form" onSubmit={this.handleSubmit}>
          <div className="row align-items-center">
            <div className="col-2">
              <Upload name="avatar" listType="picture-card" className="avatar-uploader" showUploadList={false} onChange={this.handleUploadAvatar}>
                {dataCanBo.avatar ? (
                  <img src={dataCanBo.avatar} alt="avatar" style={{ width: "100%" }} />
                ) : (
                  <div>
                    <Icon type="plus" />
                    <div className="ant-upload-text">Upload</div>
                  </div>
                )}
              </Upload>
            </div>
            <div className="col-10">
              <div className="row ">
                <div className="col-lg-6 col-12 my-1  ">
                  <div className="row align-items-center">
                    <div className="col-6 col-lg-4 ">Mã cán bộ</div>
                    <div className="col-6 col-lg-8 ">
                      <GoInput
                        //    onChange={this.handleInputChange("MaDeXuat")}
                        //    value={dataDeXuat.NoiDungKQNC}
                        validators={["required"]}
                        errorMessages={["Nội dung bắt buộc"]}
                      />
                    </div>
                  </div>
                </div>
                <div className="col-lg-6 col-12 my-1  ">
                  <div className="row align-items-center">
                    <div className="col-6 col-lg-4 ">Tên cán bộ</div>
                    <div className="col-6 col-lg-8 ">
                      <GoInput
                        //    onChange={this.handleInputChange("MaDeXuat")}
                        //    value={dataDeXuat.NoiDungKQNC}
                        validators={["required"]}
                        errorMessages={["Nội dung bắt buộc"]}
                      />
                    </div>
                  </div>
                </div>
                <div className="col-lg-6 col-12 my-1  ">
                  <div className="row align-items-center">
                    <div className="col-6 col-lg-4 ">Ngày sinh</div>
                    <div className="col-6 col-lg-8 ">
                      <GoInput
                        //    onChange={this.handleInputChange("MaDeXuat")}
                        //    value={dataDeXuat.NoiDungKQNC}
                        validators={["required"]}
                        errorMessages={["Nội dung bắt buộc"]}
                      />
                    </div>
                  </div>
                </div>
                <div className="col-lg-6 col-12 my-1  ">
                  <div className="row align-items-center">
                    <div className="col-6 col-lg-4 ">Giới tính</div>
                    <div className="col-6 col-lg-8 "></div>
                  </div>
                </div>
                <div className="col-lg-6 col-12 my-1  ">
                  <div className="row align-items-center">
                    <div className="col-6 col-lg-4 ">Chức danh khoa học</div>
                    <div className="col-6 col-lg-8 ">
                      <GoInput
                        //    onChange={this.handleInputChange("MaDeXuat")}
                        //    value={dataDeXuat.NoiDungKQNC}
                        validators={["required"]}
                        errorMessages={["Nội dung bắt buộc"]}
                      />
                    </div>
                  </div>
                </div>
                <div className="col-lg-6 col-12 my-1  ">
                  <div className="row align-items-center">
                    <div className="col-6 col-lg-4 ">Cơ quan công tác</div>
                    <div className="col-6 col-lg-8 ">
                      <GoInput
                        //    onChange={this.handleInputChange("MaDeXuat")}
                        //    value={dataDeXuat.NoiDungKQNC}
                        validators={["required"]}
                        errorMessages={["Nội dung bắt buộc"]}
                      />
                    </div>
                  </div>
                </div>
                <div className="col-lg-6 col-12 my-1  ">
                  <div className="row align-items-center">
                    <div className="col-6 col-lg-4 ">Địa chỉ cơ quan</div>
                    <div className="col-6 col-lg-8 ">
                      <GoInput
                        //    onChange={this.handleInputChange("MaDeXuat")}
                        //    value={dataDeXuat.NoiDungKQNC}
                        validators={["required"]}
                        errorMessages={["Nội dung bắt buộc"]}
                      />
                    </div>
                  </div>
                </div>
                <div className="col-lg-6 col-12 my-1  ">
                  <div className="row align-items-center">
                    <div className="col-6 col-lg-4 ">Email</div>
                    <div className="col-6 col-lg-8 ">
                      <GoInput
                        //    onChange={this.handleInputChange("MaDeXuat")}
                        //    value={dataDeXuat.NoiDungKQNC}
                        validators={["required"]}
                        errorMessages={["Nội dung bắt buộc"]}
                      />
                    </div>
                  </div>
                </div>
                <div className="col-lg-6 col-12 my-1  ">
                  <div className="row align-items-center">
                    <div className="col-6 col-lg-4 ">Điện thoại</div>
                    <div className="col-6 col-lg-8 ">
                      <GoInput
                        //    onChange={this.handleInputChange("MaDeXuat")}
                        //    value={dataDeXuat.NoiDungKQNC}
                        validators={["required"]}
                        errorMessages={["Nội dung bắt buộc"]}
                      />
                    </div>
                  </div>
                </div>
                <div className="col-lg-6 col-12 my-1  ">
                  <div className="row align-items-center">
                    <div className="col-6 col-lg-4 ">Điện thoại di động</div>
                    <div className="col-6 col-lg-8 ">
                      <GoInput
                        //    onChange={this.handleInputChange("MaDeXuat")}
                        //    value={dataDeXuat.NoiDungKQNC}
                        validators={["required"]}
                        errorMessages={["Nội dung bắt buộc"]}
                      />
                    </div>
                  </div>
                </div>
                <div className="col-lg-6 col-12 my-1  ">
                  <div className="row align-items-center">
                    <div className="col-6 col-lg-4 ">Fax</div>
                    <div className="col-6 col-lg-8 ">
                      <GoInput
                        //    onChange={this.handleInputChange("MaDeXuat")}
                        //    value={dataDeXuat.NoiDungKQNC}
                        validators={["required"]}
                        errorMessages={["Nội dung bắt buộc"]}
                      />
                    </div>
                  </div>
                </div>
                <div className="col-lg-6 col-12 my-1  ">
                  <div className="row align-items-center">
                    <div className="col-6 col-lg-4 ">Trạng thái</div>
                    <div className="col-6 col-lg-8 ">
                      <GoInput
                        //    onChange={this.handleInputChange("MaDeXuat")}
                        //    value={dataDeXuat.NoiDungKQNC}
                        validators={["required"]}
                        errorMessages={["Nội dung bắt buộc"]}
                      />
                    </div>
                  </div>
                </div>
                <div className="col-lg-6 col-12 my-1  ">
                  <div className="row align-items-center">
                    <div className="col-6 col-lg-4 ">Chuyên gia</div>
                    <div className="col-6 col-lg-8 "></div>
                  </div>
                </div>
                <div className="col-lg-6 col-12 my-1  ">
                  <div className="row align-items-center">
                    <div className="col-6 col-lg-4 ">File giới thiệu</div>
                    <div className="col-6 col-lg-8 ">
                      <Upload fileList={[]} onChange={({ file, fileList }) => {}}>
                        <Button>
                          <Icon type="upload" /> Chọn file
                        </Button>
                      </Upload>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </ValidatorForm>
      </Modal>
    );
  }
}
export default ThemCanBoModal;
