import MenuItemModal from "../../components/menuItems/MenuItemModal";
import MenuItemTable from "../../components/menuItems/MenuItemTable";
import { useGetMenuItemsQuery } from "../../store/api/menuItemApi";
import { useState } from "react";

function MenuItemManagement() {
  const {
    data: menuItems = [],
    isLoading,
    error,
    refetch,
  } = useGetMenuItemsQuery();

  const [formData, setFormData] = useState({
    name: "",
    description: "",
    specialTag: "",
    category: "",
    price: "",
    image: null,
  });
  const [showModal, setShowModal] = useState(false);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const handleCloseModal = () => {
    setShowModal(false);
  };

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    console.log(name, value);
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleFormSubmit = () => {
    setIsSubmitting(true);
    try {
      //call api
    } catch (error) {
      console.log(error);
    } finally {
      setIsSubmitting(false);
    }
  };
  return (
    <div className="container-fluid p-4 mx-3">
      <div className="row mb-4">
        <div className="col">
          <div className="d-flex justify-content-between align-items-center">
            <div>
              <h2>Menu Item Management</h2>
              <p className="text-muted mb-0">
                Manage your restaurant's menu items
              </p>
            </div>
            <button
              className="btn btn-primary"
              onClick={() => setShowModal(true)}
            >
              <i className="bi bi-plus-circle me-2"></i>
              Add Menu Item
            </button>
          </div>
        </div>
      </div>
      <div className="row">
        <div className="col">
          <div className="card">
            <div className="card-body">
              <MenuItemTable
                menuItems={menuItems}
                isLoading={isLoading}
                error={error}
              />
            </div>
          </div>
        </div>
      </div>
      {showModal && (
        <MenuItemModal
          formData={formData}
          onSubmit={handleFormSubmit}
          onClose={handleCloseModal}
          isSubmitting={isSubmitting}
          onChange={handleInputChange}
        />
      )}
    </div>
  );
}

export default MenuItemManagement;
