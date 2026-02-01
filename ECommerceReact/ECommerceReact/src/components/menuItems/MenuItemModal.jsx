import { CATEGORY, SPECIAL_TAG } from "../../Utility/constants";

function MenuItemModal({
  onClose,
  isSubmitting,
  onSubmit,
  formData,
  onChange,
}) {
  const handleSubmit = (e) => {
    e.preventDefault();

    if (!formData.name?.trim()) {
      console.log("Name is required");
      return;
    }
    if (!formData.category?.trim()) {
      console.log("Category is required");
      return;
    }
    if (
      !formData.price ||
      parseFloat(formData.price <= 0 || formData.price >= 1000)
    ) {
      console.log("Price is required and must be between 1-1000");
      return;
    }

    onSubmit(formData);
  };
  return (
    <>
      {/* Bootstrap Modal Backdrop */}
      <div className="modal-backdrop fade show" />

      {/* Bootstrap Modal */}
      <div
        className="modal fade show"
        style={{ display: "block" }}
        tabIndex="-1"
        role="dialog"
      >
        <div className={`modal-dialog modal-lg`} role="document">
          <div className="modal-content">
            <div className="modal-header">
              <h5 className="modal-title">Add New Menu Item</h5>
              <button
                type="button"
                className="btn-close"
                aria-label="Close"
                onClick={onClose}
              />
            </div>
            <div className="modal-body">
              <form onSubmit={handleSubmit}>
                <div className="row">
                  <div className="col-md-6">
                    <div className="mb-3">
                      <label className="form-label">Name *</label>
                      <input
                        type="text"
                        className="form-control"
                        name="name"
                        value={formData.name || ""}
                        onChange={onChange}
                      />
                    </div>
                  </div>
                  <div className="col-md-6">
                    <div className="mb-3">
                      <label className="form-label">Category *</label>
                      <select
                        className="form-select"
                        name="category"
                        value={formData.category || ""}
                        onChange={onChange}
                      >
                        <option value="">Select Category</option>
                        {CATEGORY.map((category) => (
                          <option value={category} key={category}>
                            {category}
                          </option>
                        ))}
                      </select>
                    </div>
                  </div>
                </div>

                <div className="mb-3">
                  <label className="form-label">Description</label>
                  <textarea
                    className="form-control"
                    name="description"
                    rows="3"
                    value={formData.description || ""}
                    onChange={onChange}
                  />
                </div>

                <div className="row">
                  <div className="col-md-6">
                    <div className="mb-3">
                      <label className="form-label">Price * ($)</label>
                      <input
                        type="number"
                        className="form-control"
                        name="price"
                        step="0.01"
                        min="0.01"
                        value={formData.price || ""}
                        onChange={onChange}
                      />
                    </div>
                  </div>
                  <div className="col-md-6">
                    <div className="mb-3">
                      <label className="form-label">Special Tag</label>
                      <select
                        className="form-select"
                        name="specialTag"
                        value={formData.specialTag || ""}
                        onChange={onChange}
                      >
                        {SPECIAL_TAG.map((tag) => (
                          <option value={tag} key={tag}>
                            {tag}
                          </option>
                        ))}
                      </select>
                    </div>
                  </div>
                </div>

                <div className="mb-4">
                  <label className="form-label">Image</label>
                  <input
                    type="file"
                    className="form-control"
                    name="image"
                    accept="image/*"
                  />
                  <div className="form-text">
                    Upload an image for the menu item
                  </div>
                </div>

                <div className="d-flex justify-content-end gap-2">
                  <button
                    type="button"
                    className="btn btn-secondary"
                    onClick={onClose}
                  >
                    Cancel
                  </button>
                  <button
                    type="submit"
                    className="btn btn-primary"
                    disabled={isSubmitting}
                  >
                    {isSubmitting ? (
                      <span className="spinner-border spinner-border-sm me-2" />
                    ) : (
                      <>CREATE MENU ITEM</>
                    )}
                  </button>
                </div>
              </form>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}

export default MenuItemModal;
